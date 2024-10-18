using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    // 调试脚本，在地图上传送玩家以进行更快的测试
    /// <summary>
    /// 复活点
    /// </summary>
    public class TeleportPlayer : MonoBehaviour
    {
        public KeyCode ActivateKey = KeyCode.F12;

        PlayerCharacterController m_PlayerCharacterController;

        void Awake()
        {
            m_PlayerCharacterController = FindObjectOfType<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, TeleportPlayer>(
                m_PlayerCharacterController, this);
        }

        void Update()
        {
            if (Input.GetKeyDown(ActivateKey))
            {
                m_PlayerCharacterController.transform.SetPositionAndRotation(transform.position, transform.rotation);
                Health playerHealth = m_PlayerCharacterController.GetComponent<Health>();
                if (playerHealth)
                {
                    playerHealth.Heal(999);
                }
            }
        }

    }
}