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

        [Header("显示左侧健康状况的图像组件")]
        public Image HealthBarImage;

        [Header("浮动健康栏枢轴变换")]
        public Transform HealthBarPivot;

        [Header("处于完全健康状态时，健康栏是否可见")]
        public bool HideFullHealthBar = true;

        void Update()
        {
            // update health bar value
            HealthBarImage.fillAmount = Health.CurrentHealth / Health.MaxHealth;

            // rotate health bar to face the camera/player
            HealthBarPivot.LookAt(Camera.main.transform.position);

            // hide health bar if needed
            if (HideFullHealthBar)
                HealthBarPivot.gameObject.SetActive(HealthBarImage.fillAmount != 1);
        }
    }
}