using TMPro;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [Header("显示当前健康状况的图像组件")]
        public Image HealthFillImage;
        [Header("显示当前护盾状况的图像组件")]
        public Image SheildFillImage;
        [Header("显示HP数值图像组件")]
        public TextMeshProUGUI HpText;
        [Header("显示护盾数值图像组件")]
        public TextMeshProUGUI SheildText;
        
        Health m_PlayerHealth;

        void Start()
        {
            PlayerCharacterController playerCharacterController =
                GameObject.FindObjectOfType<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, PlayerHealthBar>(
                playerCharacterController, this);

            m_PlayerHealth = playerCharacterController.GetComponent<Health>();
            DebugUtility.HandleErrorIfNullGetComponent<Health, PlayerHealthBar>(m_PlayerHealth, this,
                playerCharacterController.gameObject);
            
            LastHealth = m_PlayerHealth.CurrentHealth;
            LastShield = m_PlayerHealth.CurrentShield;
            SheildText.SetText(m_PlayerHealth.CurrentShield +"/"+ m_PlayerHealth.MaxShield);
            HpText.SetText(m_PlayerHealth.CurrentHealth +"/"+ m_PlayerHealth.MaxHealth);
        }

        private int LastHealth = 0;
        private int LastShield = 0;
        void Update()
        {
            // update health bar value
            
            if (LastHealth!=m_PlayerHealth.CurrentHealth)
            {
                LastHealth = m_PlayerHealth.CurrentHealth;
                HealthFillImage.fillAmount = (float)m_PlayerHealth.CurrentHealth / m_PlayerHealth.MaxHealth;
                SheildFillImage.gameObject.SetActive(m_PlayerHealth.MaxShield>0&&m_PlayerHealth.CurrentShield>0);
                HpText.SetText(m_PlayerHealth.CurrentHealth +"/"+ m_PlayerHealth.MaxHealth);
            }
            if (LastShield!=m_PlayerHealth.CurrentShield)
            {
                LastShield = m_PlayerHealth.CurrentShield;
                SheildFillImage.fillAmount = (float)m_PlayerHealth.CurrentShield / m_PlayerHealth.MaxShield;
                SheildFillImage.gameObject.SetActive(m_PlayerHealth.MaxShield>0&&m_PlayerHealth.CurrentShield>0);
                SheildText.SetText(m_PlayerHealth.CurrentShield +"/"+ m_PlayerHealth.MaxShield);
            }
            
        }
    }
}