
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class PositionBobbing : MonoBehaviour
    {
        [Header("项目上下移动的频率")]
        public float VerticalBobFrequency = 1f;

        [Header("物品上下移动的距离")]
        public float BobbingAmount = 0.5f;

        Vector3 m_StartPosition;

        void Start()
        {
            // Remember start position for animation
            m_StartPosition = transform.position;
        }

        void Update()
        {
            // Handle bobbing
            float bobbingAnimationPhase = ((Mathf.Sin(Time.time * VerticalBobFrequency) * 0.5f) + 0.5f) * BobbingAmount;
            transform.position = m_StartPosition + Vector3.up * bobbingAnimationPhase;
        }
    }
}