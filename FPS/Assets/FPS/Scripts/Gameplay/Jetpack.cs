using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    [RequireComponent(typeof(AudioSource))]
    public class Jetpack : MonoBehaviour
    {
        [Header("References")] [Header("喷气背包sfx的音频源")]
        public AudioSource AudioSource;

        [Header("喷气背包vfx的颗粒")] public ParticleSystem[] JetpackVfx;

        [Header("Parameters")] [Header("喷气背包一开始是否解锁")]
        public bool IsJetpackUnlockedAtStart = false;

        [Header("喷气背包推动玩家向上的力量")]
        public float JetpackAcceleration = 7f;

        [Range(0f, 1f)]
        [Header(
            "这将影响使用喷气背包取消重力值的程度，以开始更快地上升。0根本不是，1是即时的")]
        public float JetpackDownwardVelocityCancelingFactor = 1f;

        [Header("消耗所有喷气背包燃料所需的时间")]
        public float ConsumeDuration = 1.5f;

        [Header("在地面上完全加满喷气背包所需的时间")]
        public float RefillDurationGrounded = 2f;

        [Header("在空中完全加满喷气背包所需的时间")]
        public float RefillDurationInTheAir = 5f;

        [Header("上次使用后开始重新填充前的延迟")]
        public float RefillDelay = 1f;

        [Header("Audio")] [Header("使用喷气背包时播放的声音")]
        public AudioClip JetpackSfx;

        bool m_CanUseJetpack;
        PlayerCharacterController m_PlayerCharacterController;
        PlayerInputHandler m_InputHandler;
        float m_LastTimeOfUse;

        // stored ratio for jetpack resource (1 is full, 0 is empty)
        public float CurrentFillRatio { get; private set; }
        public bool IsJetpackUnlocked { get; private set; }

        public bool IsPlayergrounded() => m_PlayerCharacterController.IsGrounded;

        public UnityAction<bool> OnUnlockJetpack;

        void Start()
        {
            IsJetpackUnlocked = IsJetpackUnlockedAtStart;

            m_PlayerCharacterController = GetComponent<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController, Jetpack>(m_PlayerCharacterController,
                this, gameObject);

            m_InputHandler = GetComponent<PlayerInputHandler>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerInputHandler, Jetpack>(m_InputHandler, this, gameObject);

            CurrentFillRatio = 1f;

            AudioSource.clip = JetpackSfx;
            AudioSource.loop = true;
        }

        void Update()
        {
            // jetpack can only be used if not grounded and jump has been pressed again once in-air
            if (IsPlayergrounded())
            {
                m_CanUseJetpack = false;
            }
            else if (!m_PlayerCharacterController.HasJumpedThisFrame && m_InputHandler.GetJumpInputDown())
            {
                m_CanUseJetpack = true;
            }

            // jetpack usage
            bool jetpackIsInUse = m_CanUseJetpack && IsJetpackUnlocked && CurrentFillRatio > 0f &&
                                  m_InputHandler.GetJumpInputHeld();
            if (jetpackIsInUse)
            {
                // store the last time of use for refill delay
                m_LastTimeOfUse = Time.time;

                float totalAcceleration = JetpackAcceleration;

                // cancel out gravity
                totalAcceleration += m_PlayerCharacterController.GravityDownForce;

                if (m_PlayerCharacterController.CharacterVelocity.y < 0f)
                {
                    // handle making the jetpack compensate for character's downward velocity with bonus acceleration
                    totalAcceleration += ((-m_PlayerCharacterController.CharacterVelocity.y / Time.deltaTime) *
                                          JetpackDownwardVelocityCancelingFactor);
                }

                // apply the acceleration to character's velocity
                m_PlayerCharacterController.CharacterVelocity += Vector3.up * totalAcceleration * Time.deltaTime;

                // consume fuel
                CurrentFillRatio = CurrentFillRatio - (Time.deltaTime / ConsumeDuration);

                for (int i = 0; i < JetpackVfx.Length; i++)
                {
                    var emissionModulesVfx = JetpackVfx[i].emission;
                    emissionModulesVfx.enabled = true;
                }

                if (!AudioSource.isPlaying)
                    AudioSource.Play();
            }
            else
            {
                // refill the meter over time
                if (IsJetpackUnlocked && Time.time - m_LastTimeOfUse >= RefillDelay)
                {
                    float refillRate = 1 / (m_PlayerCharacterController.IsGrounded
                        ? RefillDurationGrounded
                        : RefillDurationInTheAir);
                    CurrentFillRatio = CurrentFillRatio + Time.deltaTime * refillRate;
                }

                for (int i = 0; i < JetpackVfx.Length; i++)
                {
                    var emissionModulesVfx = JetpackVfx[i].emission;
                    emissionModulesVfx.enabled = false;
                }

                // keeps the ratio between 0 and 1
                CurrentFillRatio = Mathf.Clamp01(CurrentFillRatio);

                if (AudioSource.isPlaying)
                    AudioSource.Stop();
            }
        }

        public bool TryUnlock()
        {
            if (IsJetpackUnlocked)
                return false;

            OnUnlockJetpack.Invoke(true);
            IsJetpackUnlocked = true;
            m_LastTimeOfUse = Time.time;
            return true;
        }
    }
}