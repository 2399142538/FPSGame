using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class FeedbackFlashHUD : MonoBehaviour
    {
        [Header("工具书类闪光灯的图像组件")]
        public Image FlashImage;

        [Header("CanvasGroup用于淡化伤害闪光，用于接受伤害结束治疗")]
        public CanvasGroup FlashCanvasGroup;

        [Header("CanvasGroup将淡化关键的健康小插曲")]
        public CanvasGroup VignetteCanvasGroup;

        [Header("受伤损坏闪光的颜色")]
        public Color DamageFlashColor;

        [Header("损坏闪光的持续时间")]
        public float DamageFlashDuration;

        [Header("伤害闪光的最大阿尔法值")]
        public float DamageFlashMaxAlpha = 1f;

        [Header("严重健康关键小插曲的最大阿尔法值")]
        public float CriticaHealthVignetteMaxAlpha = .8f;

        [Header("处于临界健康状态时，小插曲跳动的频率")]
        public float PulsatingVignetteFrequency = 4f;

        [Header("治愈治愈闪光的颜色")]
        public Color HealFlashColor;

        [Header("愈合闪光的持续时间")]
        public float HealFlashDuration;

        [Header("治愈闪光的最大阿尔法值")]
        public float HealFlashMaxAlpha = 1f;

        bool m_FlashActive;
        float m_LastTimeFlashStarted = Mathf.NegativeInfinity;
        Health m_PlayerHealth;
        GameFlowManager m_GameFlowManager;

        void Start()
        {
            // Subscribe to player damage events
            PlayerCharacterController playerCharacterController = FindObjectOfType<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, FeedbackFlashHUD>(
                playerCharacterController, this);

            m_PlayerHealth = playerCharacterController.GetComponent<Health>();
            DebugUtility.HandleErrorIfNullGetComponent<Health, FeedbackFlashHUD>(m_PlayerHealth, this,
                playerCharacterController.gameObject);

            m_GameFlowManager = FindObjectOfType<GameFlowManager>();
            DebugUtility.HandleErrorIfNullFindObject<GameFlowManager, FeedbackFlashHUD>(m_GameFlowManager, this);

            m_PlayerHealth.OnDamaged += OnTakeDamage;
            m_PlayerHealth.OnHealed += OnHealed;
        }

        void Update()
        {
            if (m_PlayerHealth.IsCritical())
            {
                VignetteCanvasGroup.gameObject.SetActive(true);
                float vignetteAlpha =
                    (1 - (m_PlayerHealth.CurrentHealth / m_PlayerHealth.MaxHealth /
                          m_PlayerHealth.CriticalHealthRatio)) * CriticaHealthVignetteMaxAlpha;

                if (m_GameFlowManager.GameIsEnding)
                    VignetteCanvasGroup.alpha = vignetteAlpha;
                else
                    VignetteCanvasGroup.alpha =
                        ((Mathf.Sin(Time.time * PulsatingVignetteFrequency) / 2) + 0.5f) * vignetteAlpha;
            }
            else
            {
                VignetteCanvasGroup.gameObject.SetActive(false);
            }


            if (m_FlashActive)
            {
                float normalizedTimeSinceDamage = (Time.time - m_LastTimeFlashStarted) / DamageFlashDuration;

                if (normalizedTimeSinceDamage < 1f)
                {
                    float flashAmount = DamageFlashMaxAlpha * (1f - normalizedTimeSinceDamage);
                    FlashCanvasGroup.alpha = flashAmount;
                }
                else
                {
                    FlashCanvasGroup.gameObject.SetActive(false);
                    m_FlashActive = false;
                }
            }
        }

        void ResetFlash()
        {
            m_LastTimeFlashStarted = Time.time;
            m_FlashActive = true;
            FlashCanvasGroup.alpha = 0f;
            FlashCanvasGroup.gameObject.SetActive(true);
        }

        void OnTakeDamage(float dmg, GameObject damageSource)
        {
            ResetFlash();
            FlashImage.color = DamageFlashColor;
        }

        void OnHealed(float amount)
        {
            ResetFlash();
            FlashImage.color = HealFlashColor;
        }
    }
}