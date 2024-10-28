using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    public enum WeaponShootType
    {
        Manual,
        Automatic,
        Charge,
    }

    [System.Serializable]
    public struct CrosshairData
    {
        [Header("将用于该武器十字准星的图像")]
        public Sprite CrosshairSprite;

        [Header("十字准线图像的大小")]
        public int CrosshairSize;

        [Header("十字准线图像的颜色")]
        public Color CrosshairColor;
    }

    [RequireComponent(typeof(AudioSource))]
    public class WeaponController : MonoBehaviour
    {
         [Header("武器类型")]
        public GunType GunTypeType;
        [Header("Information")] [Header("将在该武器的UI中显示的名称")]
        public string WeaponName;

        [Header("将在该武器的UI中显示的图像")]
        public Sprite WeaponIcon;

        [Header("十字准线的默认数据")]
        public CrosshairData CrosshairDataDefault;

        [Header("瞄准敌人时的十字准星数据")]
        public CrosshairData CrosshairDataTargetInSight;

        [Header("内部参考资料")]
        [Header("武器的根对象，这是武器未激活时将被停用的对象")]
        public GameObject WeaponRoot;

        [Header("武器尖端，射弹射击的地方")]
        public Transform WeaponMuzzle;

        [Header("武器参数")] [Header("武器的类型会影响它的射击方式")]
        public WeaponShootType ShootType;

        [Header("射弹预制件")] public ProjectileBase ProjectilePrefab;

        [Header("两次射击之间的最短持续时间")]
        public float DelayBetweenShots = 0.5f;

        [Header("随机发射子弹的锥体角度（0表示完全没有散布）")]
        public float BulletSpreadAngle = 0f;

        [Header("每次射击的子弹数量")]
        public int BulletsPerShot = 1;

        [Header("每次射击后推回武器的力量")] [Range(0f, 2f)]
        public float RecoilForce = 1;

        [Header("该武器瞄准时应用的默认视场比")] [Range(0f, 1f)]
        public float AimZoomRatio = 1f;

        [Header("当使用此武器瞄准时，适用于武器手臂的翻译")]
        public Vector3 AimOffset;

        [Header("Ammo Parameters")]
        [Header("玩家是否应该手动重新加载")]
        public bool AutomaticReload = true;
        [Header("武器上有物理夹，发射时会弹出弹药弹")]
        public bool HasPhysicalBullets = false;
        [Header("剪辑中的子弹数")]
        public int ClipSize = 30;
        [Header("子弹壳外壳")]
        public GameObject ShellCasing;
        [Header("物理弹药的武器弹出口")]
        public Transform EjectionPort;
        [Header("施加在壳体上的力")]
        [Range(0.0f, 5.0f)] public float ShellCasingEjectionForce = 2.0f;
        [Header("重用前可以生成的最大shell数")]
        [Range(1, 30)] public int ShellPoolSize = 1;

        [Header("开始重新装弹前最后一次射击后的延迟")]
        public float AmmoReloadDelay = 2f;

        [Header("一次开枪射出n个子弹的延迟时间")]
        public int AmmoReloadDelay1 = 80;
        
        [Header("枪中弹药的最大数量")]
        public int MaxAmmo = 8;

        [Header("充电参数（仅限充电武器）")]
        [Header("达到最大电荷时触发射击")]
        public bool AutomaticReleaseOnCharged;

        //没两秒重装1颗
        [Header("每秒重新装填的弹药量")]
        public float AmmoReloadRate = 1f;
        [Header("重装一次需要的时间")]
        public float MaxChargeDuration = 2f;

        [Header("开始充电时使用的初始弹药")]
        public float AmmoUsedOnStartCharge = 1f;

        [Header("电荷达到最大值时使用的额外弹药")]
        public float AmmoUsageRateWhileCharging = 1f;

        [Header("音频和视频")] 
        [Header("OnShoot动画的可选武器动画师")]
        public Animator WeaponAnimator;

        [Header("枪口闪光的预制")]
        public GameObject MuzzleFlashPrefab;

        [Header("打开spawn上的枪口闪光实例")]
        public bool UnparentMuzzleFlash;

        [Header("拍摄时播放的声音")]
        public AudioClip ShootSfx;

        [Header("切换到此武器时播放的声音")]
        public AudioClip ChangeWeaponSfx;

        [Header("连续拍摄声音")] public bool UseContinuousShootSound = false;
        public AudioClip ContinuousShootStartSfx;
        public AudioClip ContinuousShootLoopSfx;
        public AudioClip ContinuousShootEndSfx;
        AudioSource m_ContinuousShootAudioSource = null;
        bool m_WantsToShoot = false;

        public UnityAction OnShoot;
        public event Action OnShootProcessed;

        int m_CarriedPhysicalBullets;
        float m_CurrentAmmo;
        float m_LastTimeShot = Mathf.NegativeInfinity;
        public float LastChargeTriggerTimestamp { get; private set; }
        Vector3 m_LastMuzzlePosition;

        public GameObject Owner { get; set; }
        public GameObject SourcePrefab { get; set; }
        public bool IsCharging { get; private set; }
        public float CurrentAmmoRatio { get; private set; }
        public bool IsWeaponActive { get; private set; }
        public bool IsCooling { get; private set; }
        public float CurrentCharge { get; private set; }
        public Vector3 MuzzleWorldVelocity { get; private set; }

        public float GetAmmoNeededToShoot() =>
            (ShootType != WeaponShootType.Charge ? 1f : Mathf.Max(1f, AmmoUsedOnStartCharge)) /
            (MaxAmmo * BulletsPerShot);

        public int GetCarriedPhysicalBullets() => m_CarriedPhysicalBullets;
        public int GetCurrentAmmo() => Mathf.FloorToInt(m_CurrentAmmo);

        AudioSource m_ShootAudioSource;

        public bool IsReloading { get; private set; }

        const string k_AnimAttackParameter = "Attack";

        private Queue<Rigidbody> m_PhysicalAmmoPool;

        void Awake()
        {
            m_CurrentAmmo = MaxAmmo;
            m_CarriedPhysicalBullets = HasPhysicalBullets ? ClipSize : 0;
            m_LastMuzzlePosition = WeaponMuzzle.position;

            m_ShootAudioSource = GetComponent<AudioSource>();
            DebugUtility.HandleErrorIfNullGetComponent<AudioSource, WeaponController>(m_ShootAudioSource, this,
                gameObject);

            if (UseContinuousShootSound)
            {
                m_ContinuousShootAudioSource = gameObject.AddComponent<AudioSource>();
                m_ContinuousShootAudioSource.playOnAwake = false;
                m_ContinuousShootAudioSource.clip = ContinuousShootLoopSfx;
                m_ContinuousShootAudioSource.outputAudioMixerGroup =
                    AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.WeaponShoot);
                m_ContinuousShootAudioSource.loop = true;
            }

            if (HasPhysicalBullets)
            {
                m_PhysicalAmmoPool = new Queue<Rigidbody>(ShellPoolSize);

                for (int i = 0; i < ShellPoolSize; i++)
                {
                    GameObject shell = Instantiate(ShellCasing, transform);
                    shell.SetActive(false);
                    m_PhysicalAmmoPool.Enqueue(shell.GetComponent<Rigidbody>());
                }
            }
        }

        public void AddCarriablePhysicalBullets(int count) => m_CarriedPhysicalBullets = Mathf.Max(m_CarriedPhysicalBullets + count, MaxAmmo);

        void ShootShell()
        {
            Rigidbody nextShell = m_PhysicalAmmoPool.Dequeue();

            nextShell.transform.position = EjectionPort.transform.position;
            nextShell.transform.rotation = EjectionPort.transform.rotation;
            nextShell.gameObject.SetActive(true);
            nextShell.transform.SetParent(null);
            nextShell.collisionDetectionMode = CollisionDetectionMode.Continuous;
            nextShell.AddForce(nextShell.transform.up * ShellCasingEjectionForce, ForceMode.Impulse);

            m_PhysicalAmmoPool.Enqueue(nextShell);
        }

        void PlaySFX(AudioClip sfx) => AudioUtility.CreateSFX(sfx, transform.position, AudioUtility.AudioGroups.WeaponShoot, 0.0f);


        void Reload()
        {
            if (m_CarriedPhysicalBullets > 0)
            {
                m_CurrentAmmo = Mathf.Min(m_CarriedPhysicalBullets, ClipSize);
            }

            IsReloading = false;
        }

        public void StartReloadAnimation()
        {
            if (m_CurrentAmmo < m_CarriedPhysicalBullets)
            {
                GetComponent<Animator>().SetTrigger("Reload");
                IsReloading = true;
            }
        }

        void Update()
        {
            UpdateAmmo();
            UpdateCharge();
            UpdateContinuousShootSound();

            if (Time.deltaTime > 0)
            {
                MuzzleWorldVelocity = (WeaponMuzzle.position - m_LastMuzzlePosition) / Time.deltaTime;
                m_LastMuzzlePosition = WeaponMuzzle.position;
            }
        }

        void UpdateAmmo()
        {
            MaxAmmo = (int)GameData.instance.GetDJC(GunTypeType);
            AmmoReloadRate = GameData.instance.GetDEYRS(GunTypeType);
            if (AutomaticReload && m_LastTimeShot + AmmoReloadDelay < Time.time && m_CurrentAmmo < MaxAmmo && !IsCharging)
            {
                // reloads weapon over time
                m_CurrentAmmo += AmmoReloadRate * Time.deltaTime;

                // limits ammo to max value
                m_CurrentAmmo = Mathf.Clamp(m_CurrentAmmo, 0, MaxAmmo);

                IsCooling = true;
            }
            else
            {
                IsCooling = false;
            }

            if (MaxAmmo == Mathf.Infinity)
            {
                CurrentAmmoRatio = 1f;
            }
            else
            {
                CurrentAmmoRatio = m_CurrentAmmo / MaxAmmo;
            }
        }

        void UpdateCharge()
        {
            if (IsCharging)
            {
                if (CurrentCharge < 1f)
                {
                    float chargeLeft = 1f - CurrentCharge;

                    
                    //计算添加此框架的收费比例
                    float chargeAdded = 0f;
                    if (MaxChargeDuration <= 0f)
                    {
                        chargeAdded = chargeLeft;
                    }
                    else
                    {
                        chargeAdded = (1f / MaxChargeDuration) * Time.deltaTime;
                    }

                    chargeAdded = Mathf.Clamp(chargeAdded, 0f, chargeLeft);

                    // 看看我们是否真的可以增加这笔费用
                    float ammoThisChargeWouldRequire = chargeAdded * AmmoUsageRateWhileCharging;
                    if (ammoThisChargeWouldRequire <= m_CurrentAmmo)
                    {
                        //根据添加的电荷使用弹药
                        UseAmmo(ammoThisChargeWouldRequire);

                        // 设定电流充电比
                        CurrentCharge = Mathf.Clamp01(CurrentCharge + chargeAdded);
                    }
                }
            }
        }

        void UpdateContinuousShootSound()
        {
            if (UseContinuousShootSound)
            {
                if (m_WantsToShoot && m_CurrentAmmo >= 1f)
                {
                    if (!m_ContinuousShootAudioSource.isPlaying)
                    {
                        m_ShootAudioSource.PlayOneShot(ShootSfx);
                        m_ShootAudioSource.PlayOneShot(ContinuousShootStartSfx);
                        m_ContinuousShootAudioSource.Play();
                    }
                }
                else if (m_ContinuousShootAudioSource.isPlaying)
                {
                    m_ShootAudioSource.PlayOneShot(ContinuousShootEndSfx);
                    m_ContinuousShootAudioSource.Stop();
                }
            }
        }

        public void ShowWeapon(bool show)
        {
            WeaponRoot.SetActive(show);

            if (show && ChangeWeaponSfx)
            {
                m_ShootAudioSource.PlayOneShot(ChangeWeaponSfx);
            }

            IsWeaponActive = show;
        }

        public void UseAmmo(float amount)
        {
            m_CurrentAmmo = Mathf.Clamp(m_CurrentAmmo - amount, 0f, MaxAmmo);
            m_CarriedPhysicalBullets -= Mathf.RoundToInt(amount);
            m_CarriedPhysicalBullets = Mathf.Clamp(m_CarriedPhysicalBullets, 0, MaxAmmo);
            m_LastTimeShot = Time.time;
        }

        public bool HandleShootInputs(bool inputDown, bool inputHeld, bool inputUp)
        {
            m_WantsToShoot = inputDown || inputHeld;
            switch (ShootType)
            {
                case WeaponShootType.Manual:
                    if (inputDown)
                    {
                        return TryShoot();
                    }

                    return false;

                case WeaponShootType.Automatic:
                    if (inputHeld)
                    {
                        return TryShoot();
                    }

                    return false;

                case WeaponShootType.Charge:
                    if (inputHeld)
                    {
                        TryBeginCharge();
                    }

                    //检查我们是否释放了电荷，或者武器在充满电后是否自动射击
                    if (inputUp || (AutomaticReleaseOnCharged && CurrentCharge >= 1f))
                    {
                        return TryReleaseCharge();
                    }

                    return false;

                default:
                    return false;
            }
        }

        bool TryShoot()
        {
            if (m_CurrentAmmo >= 1f
                && m_LastTimeShot + DelayBetweenShots < Time.time)
            {
                HandleShoot();
                m_CurrentAmmo -= 1f;

                return true;
            }

            return false;
        }

        bool TryBeginCharge()
        {
            if (!IsCharging
                && m_CurrentAmmo >= AmmoUsedOnStartCharge
                && Mathf.FloorToInt((m_CurrentAmmo - AmmoUsedOnStartCharge) * BulletsPerShot) > 0
                && m_LastTimeShot + DelayBetweenShots < Time.time)
            {
                UseAmmo(AmmoUsedOnStartCharge);

                LastChargeTriggerTimestamp = Time.time;
                IsCharging = true;

                return true;
            }

            return false;
        }

        bool TryReleaseCharge()
        {
            if (IsCharging)
            {
                HandleShoot();

                CurrentCharge = 0f;
                IsCharging = false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// 手柄射击
        /// </summary>
        void HandleShoot()
        {
            int bulletsPerShotFinal = ShootType == WeaponShootType.Charge
                ? Mathf.CeilToInt(CurrentCharge * BulletsPerShot)
                : BulletsPerShot;

            // spawn all bullets with random direction
            for (int i = 0; i < bulletsPerShotFinal; i++)
            {
                Vector3 shotDirection = GetShotDirectionWithinSpread(WeaponMuzzle);
                switch (GunTypeType)
                {
                    case GunType.ChongFeng:
                    case GunType.SanDan:
                    case GunType.PaoQiang:
                        int count=Mathf.RoundToInt(GameData.instance.GetPlayerMaxData(5)) ;
                        AsyncShoot(count,shotDirection);
                        break;
                    default:
                        ProjectileBase newProjectile = Instantiate(ProjectilePrefab, WeaponMuzzle.position,
                            Quaternion.LookRotation(shotDirection));
                        newProjectile.Shoot(this);
                        break;
                }
            }

            // muzzle flash
            if (MuzzleFlashPrefab != null)
            {
                GameObject muzzleFlashInstance = Instantiate(MuzzleFlashPrefab, WeaponMuzzle.position,
                    WeaponMuzzle.rotation, WeaponMuzzle.transform);
                // Unparent the muzzleFlashInstance
                if (UnparentMuzzleFlash)
                {
                    muzzleFlashInstance.transform.SetParent(null);
                }

                Destroy(muzzleFlashInstance, 2f);
            }

            if (HasPhysicalBullets)
            {
                ShootShell();
                m_CarriedPhysicalBullets--;
            }

            m_LastTimeShot = Time.time;

            // play shoot SFX
            if (ShootSfx && !UseContinuousShootSound)
            {
                m_ShootAudioSource.PlayOneShot(ShootSfx);
            }

            // Trigger attack animation if there is any
            if (WeaponAnimator)
            {
                WeaponAnimator.SetTrigger(k_AnimAttackParameter);
            }

            OnShoot?.Invoke();
            OnShootProcessed?.Invoke();
        }


        public async void AsyncShoot(int count, Vector3 shotDirection )
        {
            for (int i = 0; i < count; i++)
            {
                ProjectileBase newProjectile = Instantiate(ProjectilePrefab, WeaponMuzzle.position,
                    Quaternion.LookRotation(shotDirection));
                newProjectile.Shoot(this);
                await Task.Delay(AmmoReloadDelay1);
            }

        }
        public Vector3 GetShotDirectionWithinSpread(Transform shootTransform)
        {
            float spreadAngleRatio = BulletSpreadAngle / 180f;
            Vector3 spreadWorldDirection = Vector3.Slerp(shootTransform.forward, UnityEngine.Random.insideUnitSphere,
                spreadAngleRatio);

            return spreadWorldDirection;
        }
    }
}