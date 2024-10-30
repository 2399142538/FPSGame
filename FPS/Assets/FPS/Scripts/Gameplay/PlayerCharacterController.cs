using Unity.FPS.Game;
using Unity.FPS;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    
    public enum PlayerStanceStage
    {
        Standing=1,
        Crouching=2,
        Runing=3
    }
    
    [RequireComponent(typeof(CharacterController), typeof(PlayerInputHandler), typeof(AudioSource))]
    public class PlayerCharacterController : MonoBehaviour
    {
        [Header("References")] [Header("参考玩家使用的主摄像头")]
        public Camera PlayerCamera;

        [Header("脚步声、跳跃声等的音频源。。。")]
        public AudioSource AudioSource;

        [Header("General")] [Header("在空中向下施加的力")]
        public float GravityDownForce = 20f;

        [Header("检查物理层以考虑玩家停飞")]
        public LayerMask GroundCheckLayers = -1;

        [Header("从字符控制器胶囊底部到接地测试的距离")]
        public float GroundCheckDistance = 0.05f;

        [Header("Movement")] [Header("落地时的最大移动速度（非短跑时）")]
        public float MaxSpeedOnGround = 10f;
        
        [Header("Movement")] [Header("落地时的最大移动速度系数减速（非短跑时）")]
        public float MaxSpeedCoefficient = 1;

        [Header("Movement")] [Header("落地时的最大移动速度系数加速（非短跑时）")]
        public float MaxSpeedCoefficient2 = 1;

        [Header(
            "落地时动作的锐度，低值会使玩家缓慢加速和减速，高值则相反")]
        public float MovementSharpnessOnGround = 15;

        [Header("蹲下时的最大移动速度")] [Range(0, 1)]
        public float MaxSpeedCrouchedRatio = 0.5f;

        [Header("未接地时的最大移动速度")]
        public float MaxSpeedInAir = 10f;

        [Header("空中加速速度")]
        public float AccelerationSpeedInAir = 25f;

        [Header("冲刺速度乘数（基于地面速度）")]
        public float SprintSpeedModifier = 2f;

        [Header("玩家从地图上跌落时瞬间死亡的高度")]
        public float KillHeight = -50f;

        [Header("Rotation")] [Header("移动相机的旋转速度")]
        public float RotationSpeed = 200f;

        [Range(0.1f, 1f)] [Header("瞄准时的转速倍增器")]
        public float AimingRotationMultiplier = 0.4f;

        [Header("Jump")] [Header("跳跃时向上施加的力")]
        public float JumpForce = 9f;

        [Header("Stance")] [Header("摄像机所在位置的角色高度比（0-1）")]
        public float CameraHeightRatio = 0.9f;

        [Header("站立时的字符高度")]
        public float CapsuleHeightStanding = 1.8f;

        [Header("人物蹲下时的身高")]
        public float CapsuleHeightCrouching = 0.9f;

        [Header("蹲下过渡的速度")]
        public float CrouchingSharpness = 10f;

        [Header("Audio")] [Header("移动一米时播放的脚步声量")]
        public float FootstepSfxFrequency = 1f;

        [Header("短跑时移动一米时发出的脚步声数量")]
        public float FootstepSfxFrequencyWhileSprinting = 1f;

        [Header("脚步声响起")]
        public AudioClip FootstepSfx;

        [Header("跳跃时发出的声音")] public AudioClip JumpSfx;
        [Header("着陆时播放的声音")] public AudioClip LandSfx;

        [Header("跌倒受伤时发出的声音")]
        public AudioClip FallDamageSfx;

        [Header("坠落伤害")]
        [Header("玩家高速撞击地面时是否会受到伤害")]
        public bool RecievesFallDamage;

        [Header("受到坠落伤害的最小坠落速度")]
        public float MinSpeedForFallDamage = 10f;

        [Header("承受最大坠落伤害的坠落速度")]
        public float MaxSpeedForFallDamage = 30f;

        [Header("以最低速度坠落时受到的伤害")]
        public float FallDamageAtMinSpeed = 10f;

        [Header("以最大速度坠落时受到的伤害")]
        public float FallDamageAtMaxSpeed = 50f;

        public UnityAction<PlayerStanceStage> OnStanceChanged;

        public Vector3 CharacterVelocity { get; set; }
        /// <summary>
        /// 是否接地
        /// </summary>
        public bool IsGrounded { get; private set; }
        public bool HasJumpedThisFrame { get; private set; }
        public bool IsDead { get; private set; }
        //是否蹲着
        public PlayerStanceStage IsCrouching { get; private set; }

        public float RotationMultiplier
        {
            get
            {
                if (m_WeaponsManager.IsAiming)
                {
                    return AimingRotationMultiplier;
                }

                return 1f;
            }
        }

        Health m_Health;
        PlayerInputHandler m_InputHandler;
        CharacterController m_Controller;
        PlayerWeaponsManager m_WeaponsManager;
        Actor m_Actor;
        Vector3 m_GroundNormal;
        Vector3 m_CharacterVelocity;
        Vector3 m_LatestImpactSpeed;
        float m_LastTimeJumped = 0f;
        float m_CameraVerticalAngle = 0f;
        float m_FootstepDistanceCounter;
        float m_TargetCharacterHeight;

        const float k_JumpGroundingPreventionTime = 0.2f;
        const float k_GroundCheckDistanceInAir = 0.07f;

        void Awake()
        {
            ActorsManager actorsManager = FindObjectOfType<ActorsManager>();
            if (actorsManager != null)
                actorsManager.SetPlayer(gameObject);
        }

        void Start()
        {
            // fetch components on the same gameObject
            m_Controller = GetComponent<CharacterController>();
            DebugUtility.HandleErrorIfNullGetComponent<CharacterController, PlayerCharacterController>(m_Controller,
                this, gameObject);

            m_InputHandler = GetComponent<PlayerInputHandler>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerInputHandler, PlayerCharacterController>(m_InputHandler,
                this, gameObject);

            m_WeaponsManager = GetComponent<PlayerWeaponsManager>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerWeaponsManager, PlayerCharacterController>(
                m_WeaponsManager, this, gameObject);

            m_Health = GetComponent<Health>();
            DebugUtility.HandleErrorIfNullGetComponent<Health, PlayerCharacterController>(m_Health, this, gameObject);

            m_Actor = GetComponent<Actor>();
            DebugUtility.HandleErrorIfNullGetComponent<Actor, PlayerCharacterController>(m_Actor, this, gameObject);

            m_Controller.enableOverlapRecovery = true;
            m_Health.MaxHealth =Mathf.RoundToInt( GameData.instance.GetPlayerMaxData(1));
            m_Health.MaxShield =Mathf.RoundToInt( GameData.instance.GetPlayerMaxData(2));
            m_Health.ShieldRecoveryCount =Mathf.RoundToInt( GameData.instance.GetPlayerMaxData(3));
            m_Health.Init();
            m_Health.OnDie += OnDie;

            // force the crouch state to false when starting
            SetCrouchingState(PlayerStanceStage.Standing, true);
            UpdateCharacterHeight(true);
        }

        void Update()
        {
            // check for Y kill
            if (!IsDead && transform.position.y < KillHeight)
            {
                m_Health.Kill();
            }

            HasJumpedThisFrame = false;

            bool wasGrounded = IsGrounded;
            GroundCheck();

            // landing
            if (IsGrounded && !wasGrounded)
            {
                // Fall damage
                float fallSpeed = -Mathf.Min(CharacterVelocity.y, m_LatestImpactSpeed.y);
                float fallSpeedRatio = (fallSpeed - MinSpeedForFallDamage) /
                                       (MaxSpeedForFallDamage - MinSpeedForFallDamage);
                if (RecievesFallDamage && fallSpeedRatio > 0f)
                {
                    float dmgFromFall = Mathf.Lerp(FallDamageAtMinSpeed, FallDamageAtMaxSpeed, fallSpeedRatio);
                    m_Health.TakeDamage(Mathf.RoundToInt(dmgFromFall), null);

                    // fall damage SFX
                    AudioSource.PlayOneShot(FallDamageSfx);
                }
                else
                {
                    // land SFX
                    AudioSource.PlayOneShot(LandSfx);
                }
            }

            // crouching
            if (m_InputHandler.GetCrouchInputDown())
            {
                if (IsCrouching==PlayerStanceStage.Standing)
                {
                    IsCrouching = PlayerStanceStage.Crouching;
                    SetCrouchingState(IsCrouching, false);
                }
                else if (IsCrouching==PlayerStanceStage.Crouching)
                {
                    IsCrouching = PlayerStanceStage.Standing;
                    SetCrouchingState(IsCrouching, false);
                }
  
            }

            UpdateCharacterHeight(false);

            HandleCharacterMovement();
        }

        void OnDie()
        {
            IsDead = true;

            // Tell the weapons manager to switch to a non-existing weapon in order to lower the weapon
            m_WeaponsManager.SwitchToWeaponIndex(-1, true);

            EventManager.Broadcast(Events.PlayerDeathEvent);
        }

        void GroundCheck()
        {
            // Make sure that the ground check distance while already in air is very small, to prevent suddenly snapping to ground
            float chosenGroundCheckDistance =
                IsGrounded ? (m_Controller.skinWidth + GroundCheckDistance) : k_GroundCheckDistanceInAir;

            // reset values before the ground check
            IsGrounded = false;
            m_GroundNormal = Vector3.up;

            // only try to detect ground if it's been a short amount of time since last jump; otherwise we may snap to the ground instantly after we try jumping
            if (Time.time >= m_LastTimeJumped + k_JumpGroundingPreventionTime)
            {
                // if we're grounded, collect info about the ground normal with a downward capsule cast representing our character capsule
                if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(m_Controller.height),
                    m_Controller.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance, GroundCheckLayers,
                    QueryTriggerInteraction.Ignore))
                {
                    // storing the upward direction for the surface found
                    m_GroundNormal = hit.normal;

                    // Only consider this a valid ground hit if the ground normal goes in the same direction as the character up
                    // and if the slope angle is lower than the character controller's limit
                    if (Vector3.Dot(hit.normal, transform.up) > 0f &&
                        IsNormalUnderSlopeLimit(m_GroundNormal))
                    {
                        IsGrounded = true;

                        // handle snapping to the ground
                        if (hit.distance > m_Controller.skinWidth)
                        {
                            m_Controller.Move(Vector3.down * hit.distance);
                        }
                    }
                }
            }
        }

        void HandleCharacterMovement()
        {
            // horizontal character rotation
            {
                // rotate the transform with the input speed around its local Y axis
                transform.Rotate(
                    new Vector3(0f, (m_InputHandler.GetLookInputsHorizontal() * RotationSpeed * RotationMultiplier),
                        0f), Space.Self);
            }

            // vertical camera rotation
            {
                // add vertical inputs to the camera's vertical angle
                m_CameraVerticalAngle += m_InputHandler.GetLookInputsVertical() * RotationSpeed * RotationMultiplier;

                // limit the camera's vertical angle to min/max
                m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -89f, 89f);

                // apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
                PlayerCamera.transform.localEulerAngles = new Vector3(m_CameraVerticalAngle, 0, 0);
            }

            // character movement handling
            bool isSprinting = m_InputHandler.GetSprintInputHeld();
            {
                if (isSprinting)
                {
                    IsCrouching = PlayerStanceStage.Runing;
                    SetCrouchingState(PlayerStanceStage.Runing, false);
                }
                else if (IsCrouching!=PlayerStanceStage.Crouching)
                {
                    IsCrouching = PlayerStanceStage.Standing;
                    SetCrouchingState(PlayerStanceStage.Standing, false);
                }

                float speedModifier = isSprinting ? SprintSpeedModifier : 1f;

                // converts move input to a worldspace vector based on our character's transform orientation
                Vector3 worldspaceMoveInput = transform.TransformVector(m_InputHandler.GetMoveInput());

                // handle grounded movement
                if (IsGrounded)
                {
                    // calculate the desired velocity from inputs, max speed, and current slope
                    Vector3 targetVelocity = worldspaceMoveInput * MaxSpeedOnGround*(MaxSpeedCoefficient)*(MaxSpeedCoefficient2) * speedModifier;
                    // reduce speed if crouching by crouch speed ratio
                                    
                    if (IsCrouching==PlayerStanceStage.Crouching) targetVelocity *= MaxSpeedCrouchedRatio;
                    targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, m_GroundNormal) *
                                     targetVelocity.magnitude;

                    // smoothly interpolate between our current velocity and the target velocity based on acceleration speed
                    CharacterVelocity = Vector3.Lerp(CharacterVelocity, targetVelocity,
                        MovementSharpnessOnGround * Time.deltaTime);

                    // jumping
                    if (IsGrounded && m_InputHandler.GetJumpInputDown())
                    {
                        // 强制将蹲伏状态设置为假
                        if (SetCrouchingState(PlayerStanceStage.Standing, false))
                        {
                            // 首先抵消我们速度的垂直分量
                            CharacterVelocity = new Vector3(CharacterVelocity.x, 0f, CharacterVelocity.z);

                            //然后，向上添加jumpSpeed值
                            CharacterVelocity += Vector3.up * JumpForce;

                            // play sound
                            AudioSource.PlayOneShot(JumpSfx);

                            // remember last time we jumped because we need to prevent snapping to ground for a short time
                            m_LastTimeJumped = Time.time;
                            HasJumpedThisFrame = true;

                            // Force grounding to false
                            IsGrounded = false;
                            m_GroundNormal = Vector3.up;
                        }
                    }

                    // footsteps sound
                    float chosenFootstepSfxFrequency =
                        (isSprinting ? FootstepSfxFrequencyWhileSprinting : FootstepSfxFrequency);
                    if (m_FootstepDistanceCounter >= 1f / chosenFootstepSfxFrequency)
                    {
                        m_FootstepDistanceCounter = 0f;
                        AudioSource.PlayOneShot(FootstepSfx);
                    }

                    // keep track of distance traveled for footsteps sound
                    m_FootstepDistanceCounter += CharacterVelocity.magnitude * Time.deltaTime;
                }
                // handle air movement
                else
                {
                    // add air acceleration
                    CharacterVelocity += worldspaceMoveInput * AccelerationSpeedInAir * Time.deltaTime;

                    // limit air speed to a maximum, but only horizontally
                    float verticalVelocity = CharacterVelocity.y;
                    Vector3 horizontalVelocity = Vector3.ProjectOnPlane(CharacterVelocity, Vector3.up);
                    horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, MaxSpeedInAir * speedModifier);
                    CharacterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

                    // apply the gravity to the velocity
                    CharacterVelocity += Vector3.down * GravityDownForce * Time.deltaTime;
                }
            }

            // apply the final calculated velocity value as a character movement
            Vector3 capsuleBottomBeforeMove = GetCapsuleBottomHemisphere();
            Vector3 capsuleTopBeforeMove = GetCapsuleTopHemisphere(m_Controller.height);
            m_Controller.Move(CharacterVelocity * Time.deltaTime);

            // detect obstructions to adjust velocity accordingly
            m_LatestImpactSpeed = Vector3.zero;
            if (Physics.CapsuleCast(capsuleBottomBeforeMove, capsuleTopBeforeMove, m_Controller.radius,
                CharacterVelocity.normalized, out RaycastHit hit, CharacterVelocity.magnitude * Time.deltaTime, -1,
                QueryTriggerInteraction.Ignore))
            {
                // We remember the last impact speed because the fall damage logic might need it
                m_LatestImpactSpeed = CharacterVelocity;

                CharacterVelocity = Vector3.ProjectOnPlane(CharacterVelocity, hit.normal);
            }
        }

        // Returns true if the slope angle represented by the given normal is under the slope angle limit of the character controller
        bool IsNormalUnderSlopeLimit(Vector3 normal)
        {
            return Vector3.Angle(transform.up, normal) <= m_Controller.slopeLimit;
        }

        // Gets the center point of the bottom hemisphere of the character controller capsule    
        Vector3 GetCapsuleBottomHemisphere()
        {
            return transform.position + (transform.up * m_Controller.radius);
        }

        // Gets the center point of the top hemisphere of the character controller capsule    
        Vector3 GetCapsuleTopHemisphere(float atHeight)
        {
            return transform.position + (transform.up * (atHeight - m_Controller.radius));
        }

        // Gets a reoriented direction that is tangent to a given slope
        public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
        {
            Vector3 directionRight = Vector3.Cross(direction, transform.up);
            return Vector3.Cross(slopeNormal, directionRight).normalized;
        }

        void UpdateCharacterHeight(bool force)
        {
            // Update height instantly
            if (force)
            {
                m_Controller.height = m_TargetCharacterHeight;
                m_Controller.center = Vector3.up * m_Controller.height * 0.5f;
                PlayerCamera.transform.localPosition = Vector3.up * m_TargetCharacterHeight * CameraHeightRatio;
                m_Actor.AimPoint.transform.localPosition = m_Controller.center;
            }
            // Update smooth height
            else if (m_Controller.height != m_TargetCharacterHeight)
            {
                // resize the capsule and adjust camera position
                m_Controller.height = Mathf.Lerp(m_Controller.height, m_TargetCharacterHeight,
                    CrouchingSharpness * Time.deltaTime);
                m_Controller.center = Vector3.up * m_Controller.height * 0.5f;
                PlayerCamera.transform.localPosition = Vector3.Lerp(PlayerCamera.transform.localPosition,
                    Vector3.up * m_TargetCharacterHeight * CameraHeightRatio, CrouchingSharpness * Time.deltaTime);
                m_Actor.AimPoint.transform.localPosition = m_Controller.center;
            }
        }

        // 如果存在障碍，则返回false
        bool SetCrouchingState(PlayerStanceStage crouched, bool ignoreObstructions)
        {
            // 设置适当的高度

            switch (crouched)
            {
                case PlayerStanceStage.Crouching:
                    m_TargetCharacterHeight = CapsuleHeightCrouching;
                    break;
                case PlayerStanceStage.Runing:
                case PlayerStanceStage.Standing:
                    // 检测障碍物
                    if (!ignoreObstructions)
                    {
                        Collider[] standingOverlaps = Physics.OverlapCapsule(
                            GetCapsuleBottomHemisphere(),
                            GetCapsuleTopHemisphere(CapsuleHeightStanding),
                            m_Controller.radius,
                            -1,
                            QueryTriggerInteraction.Ignore);
                        foreach (Collider c in standingOverlaps)
                        {
                            if (c != m_Controller)
                            {
                                return false;
                            }
                        }
                    }

                    m_TargetCharacterHeight = CapsuleHeightStanding;
                    break;
            }
            
  
            if (OnStanceChanged != null)
            {
                OnStanceChanged.Invoke(crouched);
            }

            IsCrouching = crouched;
            return true;
        }
    }
}