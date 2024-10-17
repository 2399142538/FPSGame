using Unity.FPS.AI;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class EnemyCounter : MonoBehaviour
    {
        [Header("敌人用于显示敌方目标进度的文本组件")]
        public Text EnemiesText;

        EnemyManager m_EnemyManager;

        void Awake()
        {
            m_EnemyManager = FindObjectOfType<EnemyManager>();
            DebugUtility.HandleErrorIfNullFindObject<EnemyManager, EnemyCounter>(m_EnemyManager, this);
        }

        void Update()
        {
            EnemiesText.text = m_EnemyManager.NumberOfEnemiesRemaining + "/" + m_EnemyManager.NumberOfEnemiesTotal;
        }
    }
}