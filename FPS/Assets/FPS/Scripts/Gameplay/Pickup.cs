using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    /// <summary>
    /// 拾取物体父类
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Pickup : MonoBehaviour
    {
        [Header("项目上下移动的频率")]
        public float VerticalBobFrequency = 1f;

        [Header("物品上下移动的距离")]
        public float BobbingAmount = 1f;

        [Header("每秒旋转角度")] public float RotatingSpeed = 360f;

        [Header("拾音器上播放的声音")] public AudioClip PickupSfx;
        [Header("视觉特效在拾取时产生")] public GameObject PickupVfxPrefab;

        public Rigidbody PickupRigidbody { get; private set; }

        Collider m_Collider;
        Vector3 m_StartPosition;
        bool m_HasPlayedFeedback;

        protected virtual void Start()
        {
            PickupRigidbody = GetComponent<Rigidbody>();
            DebugUtility.HandleErrorIfNullGetComponent<Rigidbody, Pickup>(PickupRigidbody, this, gameObject);
            m_Collider = GetComponent<Collider>();
            DebugUtility.HandleErrorIfNullGetComponent<Collider, Pickup>(m_Collider, this, gameObject);

            // ensure the physics setup is a kinematic rigidbody trigger
            PickupRigidbody.isKinematic = true;
            m_Collider.isTrigger = true;

            // Remember start position for animation
            m_StartPosition = transform.position;
        }

        void Update()
        {
            // Handle bobbing
            float bobbingAnimationPhase = ((Mathf.Sin(Time.time * VerticalBobFrequency) * 0.5f) + 0.5f) * BobbingAmount;
            transform.position = m_StartPosition + Vector3.up * bobbingAnimationPhase;

            // Handle rotating
            transform.Rotate(Vector3.up, RotatingSpeed * Time.deltaTime, Space.Self);
        }

        void OnTriggerEnter(Collider other)
        {
            PlayerCharacterController pickingPlayer = other.GetComponent<PlayerCharacterController>();

            if (pickingPlayer != null)
            {
                OnPicked(pickingPlayer);

                PickupEvent evt = Events.PickupEvent;
                evt.Pickup = gameObject;
                EventManager.Broadcast(evt);
            }
        }

        protected virtual void OnPicked(PlayerCharacterController playerController)
        {
            PlayPickupFeedback();
        }

        /// <summary>
        /// 播放拾取反馈
        ///
        /// </summary>
        public void PlayPickupFeedback()
        {
            if (m_HasPlayedFeedback)
                return;

            if (PickupSfx)
            {
                AudioUtility.CreateSFX(PickupSfx, transform.position, AudioUtility.AudioGroups.Pickup, 0f);
            }

            if (PickupVfxPrefab)
            {
                var pickupVfxInstance = Instantiate(PickupVfxPrefab, transform.position, Quaternion.identity);
            }

            m_HasPlayedFeedback = true;
        }
    }
}