using UnityEngine;

namespace Unity.FPS.Game
{
    public class Destructable : MonoBehaviour
    {
        Health m_Health;

        void Start()
        {
            m_Health = GetComponent<Health>();
            DebugUtility.HandleErrorIfNullGetComponent<Health, Destructable>(m_Health, this, gameObject);

            // Subscribe to damage & death actions
            m_Health.OnDie += OnDie;
            m_Health.OnDamaged += OnDamaged;
            m_Health.OnShieldDamaged += OnShieldDamaged;
            m_Health.OnShieldDie += OnShieldDie;
        }

        void OnDamaged(float damage, GameObject damageSource)
        {
            // TODO: damage reaction损伤反应
        }
        /// <summary>
        /// 护盾丢失反应
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="damageSource"></param>
        void OnShieldDamaged(float damage, GameObject damageSource)
        {
            // TODO: damage reaction损伤反应
        }

        void OnDie()
        {
            // this will call the OnDestroy function
            Destroy(gameObject);
        }
        void OnShieldDie()
        {
            //护盾破碎效果
            //Destroy(gameObject);
        }
    }
}