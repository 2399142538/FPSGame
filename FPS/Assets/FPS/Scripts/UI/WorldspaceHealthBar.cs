using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    /// <summary>
    ///  [Header("Health component to track")]血条逻辑
    /// </summary>

    public class WorldspaceHealthBar : MonoBehaviour
    {
        [Header("要跟踪的健康组件")] public Health Health;

        [Header("显示血条的图像组件")]
        public Image HealthBarImage;
        
        [Header("显示护盾条状况的图像组件")]
        public Image ShieldBarImage;
        [Header("显示护盾条背景")]
        public Image ShieldBarImageBg;

        [Header("浮动健康栏枢轴变换")]
        public Transform HealthBarPivot;

        [Header("处于完全健康状态时，健康栏是否可见")]
        public bool HideFullHealthBar = true;

        void Update()
        {
            // update health bar value
            HealthBarImage.fillAmount = Health.CurrentHealth / Health.MaxHealth;
            ShieldBarImage.fillAmount = Health.CurrentShield / Health.MaxShield;
            ShieldBarImageBg.gameObject.SetActive(Health.MaxShield>0&&Health.CurrentShield>0);
            // rotate health bar to face the camera/player
            HealthBarPivot.LookAt(Camera.main.transform.position);

            // hide health bar if needed
            if (HideFullHealthBar)
                HealthBarPivot.gameObject.SetActive(HealthBarImage.fillAmount != 1||ShieldBarImage.fillAmount!=1);
        }
    }
}