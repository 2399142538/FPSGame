using UnityEngine;
using System.Collections.Generic;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    public class OverheatBehavior : MonoBehaviour
    {
        [System.Serializable]
        public struct RendererIndexData
        {
            public Renderer Renderer;
            public int MaterialIndex;

            public RendererIndexData(Renderer renderer, int index)
            {
                this.Renderer = renderer;
                this.MaterialIndex = index;
            }
        }

        [Header("视觉")] [Header("根据弹药比缩放产卵率的VFX")]
        public ParticleSystem SteamVfx;

        [Header("完全过热时的排放率")]
        public float SteamVfxEmissionRateMax = 8f;

        //Set gradient field to HDR
        [GradientUsage(true)] [Header("基于弹药比的过热颜色")]
        public Gradient OverheatGradient;

        [Header("过热彩色动画的材质")]
        public Material OverheatingMaterial;

        [Header("Sound")] [Header("电池冷却时发出的声音")]
        public AudioClip CoolingCellsSound;

        [Header("弹药体积比曲线")]
        public AnimationCurve AmmoToVolumeRatioCurve;


        WeaponController m_Weapon;
        AudioSource m_AudioSource;
        List<RendererIndexData> m_OverheatingRenderersData;
        MaterialPropertyBlock m_OverheatMaterialPropertyBlock;
        float m_LastAmmoRatio;
        ParticleSystem.EmissionModule m_SteamVfxEmissionModule;

        void Awake()
        {
            var emissionModule = SteamVfx.emission;
            emissionModule.rateOverTimeMultiplier = 0f;

            m_OverheatingRenderersData = new List<RendererIndexData>();
            foreach (var renderer in GetComponentsInChildren<Renderer>(true))
            {
                for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                {
                    if (renderer.sharedMaterials[i] == OverheatingMaterial)
                        m_OverheatingRenderersData.Add(new RendererIndexData(renderer, i));
                }
            }

            m_OverheatMaterialPropertyBlock = new MaterialPropertyBlock();
            m_SteamVfxEmissionModule = SteamVfx.emission;

            m_Weapon = GetComponent<WeaponController>();
            DebugUtility.HandleErrorIfNullGetComponent<WeaponController, OverheatBehavior>(m_Weapon, this, gameObject);

            m_AudioSource = gameObject.AddComponent<AudioSource>();
            m_AudioSource.clip = CoolingCellsSound;
            m_AudioSource.outputAudioMixerGroup = AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.WeaponOverheat);
        }

        void Update()
        {
            // visual smoke shooting out of the gun
            float currentAmmoRatio = m_Weapon.CurrentAmmoRatio;
            if (currentAmmoRatio != m_LastAmmoRatio)
            {
                m_OverheatMaterialPropertyBlock.SetColor("_EmissionColor",
                    OverheatGradient.Evaluate(1f - currentAmmoRatio));

                foreach (var data in m_OverheatingRenderersData)
                {
                    data.Renderer.SetPropertyBlock(m_OverheatMaterialPropertyBlock, data.MaterialIndex);
                }

                m_SteamVfxEmissionModule.rateOverTimeMultiplier = SteamVfxEmissionRateMax * (1f - currentAmmoRatio);
            }

            // cooling sound
            if (CoolingCellsSound)
            {
                if (!m_AudioSource.isPlaying
                    && currentAmmoRatio != 1
                    && m_Weapon.IsWeaponActive
                    && m_Weapon.IsCooling)
                {
                    m_AudioSource.Play();
                }
                else if (m_AudioSource.isPlaying
                         && (currentAmmoRatio == 1 || !m_Weapon.IsWeaponActive || !m_Weapon.IsCooling))
                {
                    m_AudioSource.Stop();
                    return;
                }

                m_AudioSource.volume = AmmoToVolumeRatioCurve.Evaluate(1 - currentAmmoRatio);
            }

            m_LastAmmoRatio = currentAmmoRatio;
        }
    }
}