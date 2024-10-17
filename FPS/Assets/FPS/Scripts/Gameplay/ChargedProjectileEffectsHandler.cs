using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ChargedProjectileEffectsHandler : MonoBehaviour
    {
        [Header("受充电比例和颜色变化影响的物体")]
        public GameObject ChargingObject;

        [Header("基于电荷的带电物体比例")]
        public MinMaxVector3 Scale;

        [Header("基于电荷的带电物体的颜色")]
        public MinMaxColor Color;

        MeshRenderer[] m_AffectedRenderers;
        ProjectileBase m_ProjectileBase;

        void OnEnable()
        {
            m_ProjectileBase = GetComponent<ProjectileBase>();
            DebugUtility.HandleErrorIfNullGetComponent<ProjectileBase, ChargedProjectileEffectsHandler>(
                m_ProjectileBase, this, gameObject);

            m_ProjectileBase.OnShoot += OnShoot;

            m_AffectedRenderers = ChargingObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var ren in m_AffectedRenderers)
            {
                ren.sharedMaterial = Instantiate(ren.sharedMaterial);
            }
        }

        void OnShoot()
        {
            ChargingObject.transform.localScale = Scale.GetValueFromRatio(m_ProjectileBase.InitialCharge);

            foreach (var ren in m_AffectedRenderers)
            {
                ren.sharedMaterial.SetColor("_Color", Color.GetValueFromRatio(m_ProjectileBase.InitialCharge));
            }
        }
    }
}