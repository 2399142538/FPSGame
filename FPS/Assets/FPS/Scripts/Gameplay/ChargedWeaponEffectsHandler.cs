using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    [RequireComponent(typeof(AudioSource))]
    public class ChargedWeaponEffectsHandler : MonoBehaviour
    {
        [Header("视觉受充电比例和颜色变化影响的物体")]
        public GameObject ChargingObject;

        [Header("纺纱机")] public GameObject SpinningFrame;

        [Header("基于电荷的带电物体比例")]
        public MinMaxVector3 Scale;

        [Header("Particles充电时产生的粒子")]
        public GameObject DiskOrbitParticlePrefab;

        [Header("电荷粒子的局部位置偏移（相对于此变换）")]
        public Vector3 Offset;

        [Header("粒子的父变换（可选）")]
        public Transform ParentTransform;

        [Header("基于电荷的带电粒子轨道速度")]
        public MinMaxFloat OrbitY;

        [Header("基于电荷的电荷粒子半径")]
        public MinMaxVector3 Radius;

        [Header("基于电荷的机架空转速度")]
        public MinMaxFloat SpinningSpeed;

        [Header("SoundSFX充电音频夹")]
        public AudioClip ChargeSound;

        [Header("此武器在更换完毕后循环播放声音")]
        public AudioClip LoopChargeWeaponSfx;

        [Header("充电和循环声音之间的交叉衰减持续时间")]
        public float FadeLoopDuration = 0.5f;

        [Header(
            "如果为真，ChargeSound将被忽略，LoopSound上的音调将根据收费金额按程序调整")]
        public bool UseProceduralPitchOnLoopSfx;

        [Range(1.0f, 5.0f), Header("最大程序间距值")]
        public float MaxProceduralPitchValue = 2.0f;

        public GameObject ParticleInstance { get; set; }

        ParticleSystem m_DiskOrbitParticle;
        WeaponController m_WeaponController;
        ParticleSystem.VelocityOverLifetimeModule m_VelocityOverTimeModule;

        AudioSource m_AudioSource;
        AudioSource m_AudioSourceLoop;

        float m_LastChargeTriggerTimestamp;
        float m_ChargeRatio;
        float m_EndchargeTime;

        void Awake()
        {
            m_LastChargeTriggerTimestamp = 0.0f;

            // The charge effect needs it's own AudioSources, since it will play on top of the other gun sounds
            m_AudioSource = gameObject.AddComponent<AudioSource>();
            m_AudioSource.clip = ChargeSound;
            m_AudioSource.playOnAwake = false;
            m_AudioSource.outputAudioMixerGroup =
                AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.WeaponChargeBuildup);

            // create a second audio source, to play the sound with a delay
            m_AudioSourceLoop = gameObject.AddComponent<AudioSource>();
            m_AudioSourceLoop.clip = LoopChargeWeaponSfx;
            m_AudioSourceLoop.playOnAwake = false;
            m_AudioSourceLoop.loop = true;
            m_AudioSourceLoop.outputAudioMixerGroup =
                AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.WeaponChargeLoop);
        }

        void SpawnParticleSystem()
        {
            ParticleInstance = Instantiate(DiskOrbitParticlePrefab,
                ParentTransform != null ? ParentTransform : transform);
            ParticleInstance.transform.localPosition += Offset;

            FindReferences();
        }

        public void FindReferences()
        {
            m_DiskOrbitParticle = ParticleInstance.GetComponent<ParticleSystem>();
            DebugUtility.HandleErrorIfNullGetComponent<ParticleSystem, ChargedWeaponEffectsHandler>(m_DiskOrbitParticle,
                this, ParticleInstance.gameObject);

            m_WeaponController = GetComponent<WeaponController>();
            DebugUtility.HandleErrorIfNullGetComponent<WeaponController, ChargedWeaponEffectsHandler>(
                m_WeaponController, this, gameObject);

            m_VelocityOverTimeModule = m_DiskOrbitParticle.velocityOverLifetime;
        }

        void Update()
        {
            if (ParticleInstance == null)
                SpawnParticleSystem();

            m_DiskOrbitParticle.gameObject.SetActive(m_WeaponController.IsWeaponActive);
            m_ChargeRatio = m_WeaponController.CurrentCharge;

            ChargingObject.transform.localScale = Scale.GetValueFromRatio(m_ChargeRatio);
            if (SpinningFrame != null)
            {
                SpinningFrame.transform.localRotation *= Quaternion.Euler(0,
                    SpinningSpeed.GetValueFromRatio(m_ChargeRatio) * Time.deltaTime, 0);
            }

            m_VelocityOverTimeModule.orbitalY = OrbitY.GetValueFromRatio(m_ChargeRatio);
            m_DiskOrbitParticle.transform.localScale = Radius.GetValueFromRatio(m_ChargeRatio * 1.1f);

            // update sound's volume and pitch 
            if (m_ChargeRatio > 0)
            {
                if (!m_AudioSourceLoop.isPlaying &&
                    m_WeaponController.LastChargeTriggerTimestamp > m_LastChargeTriggerTimestamp)
                {
                    m_LastChargeTriggerTimestamp = m_WeaponController.LastChargeTriggerTimestamp;
                    if (!UseProceduralPitchOnLoopSfx)
                    {
                        m_EndchargeTime = Time.time + ChargeSound.length;
                        m_AudioSource.Play();
                    }

                    m_AudioSourceLoop.Play();
                }

                if (!UseProceduralPitchOnLoopSfx)
                {
                    float volumeRatio =
                        Mathf.Clamp01((m_EndchargeTime - Time.time - FadeLoopDuration) / FadeLoopDuration);
                    m_AudioSource.volume = volumeRatio;
                    m_AudioSourceLoop.volume = 1 - volumeRatio;
                }
                else
                {
                    m_AudioSourceLoop.pitch = Mathf.Lerp(1.0f, MaxProceduralPitchValue, m_ChargeRatio);
                }
            }
            else
            {
                m_AudioSource.Stop();
                m_AudioSourceLoop.Stop();
            }
        }
    }
}