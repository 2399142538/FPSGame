using TMPro;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    [RequireComponent(typeof(FillBarColorChange))]
    public class AmmoCounter : MonoBehaviour
    {
        [Header("CanvasGroup将淡化弹药UI")]
        public CanvasGroup CanvasGroup;

        [Header("武器图标图片")] public Image WeaponImage;

        [Header("背景的图像组件")]
        public Image AmmoBackgroundImage;

        [Header("图像组件显示填充率")]
        public Image AmmoFillImage;

        [Header("武器索引文本")] 
        public TextMeshProUGUI WeaponIndexText;

        [Header("子弹计数器文本")] 
        public TextMeshProUGUI BulletCounter;

        [Header("用物理子弹重新加载武器文本")]
        public RectTransform Reload;

        [Header("Selection")] [Range(0, 1)] [Header("未选择武器时的不透明度")]
        public float UnselectedOpacity = 0.5f;

        [Header("未选择武器时缩放")]
        public Vector3 UnselectedScale = Vector3.one * 0.8f;

        [Header("控制键的根")] public GameObject ControlKeysRoot;

        [Header("反馈意见")] [Header("用于在空或满时设置颜色动画的组件")]
        public FillBarColorChange FillBarColorChange;

        [Header("填充比运动的锐度")]
        public float AmmoFillMovementSharpness = 20f;

        public int WeaponCounterIndex { get; set; }

        PlayerWeaponsManager m_PlayerWeaponsManager;
        WeaponController m_Weapon;

        void Awake()
        {
            EventManager.AddListener<AmmoPickupEvent>(OnAmmoPickup);
        }

        void OnAmmoPickup(AmmoPickupEvent evt)
        {
            if (evt.Weapon == m_Weapon)
            {
                BulletCounter.text = m_Weapon.GetCarriedPhysicalBullets().ToString();
            }
        }

        public void Initialize(WeaponController weapon, int weaponIndex)
        {
            m_Weapon = weapon;
            WeaponCounterIndex = weaponIndex;
            WeaponImage.sprite = weapon.WeaponIcon;
            if (!weapon.HasPhysicalBullets)
                BulletCounter.transform.parent.gameObject.SetActive(false);
            else
                BulletCounter.text = weapon.GetCarriedPhysicalBullets().ToString();

            Reload.gameObject.SetActive(false);
            m_PlayerWeaponsManager = FindObjectOfType<PlayerWeaponsManager>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerWeaponsManager, AmmoCounter>(m_PlayerWeaponsManager, this);

            WeaponIndexText.text = (WeaponCounterIndex + 1).ToString();

            FillBarColorChange.Initialize(1f, m_Weapon.GetAmmoNeededToShoot());
        }

        void Update()
        {
            float currenFillRatio = m_Weapon.CurrentAmmoRatio;
            AmmoFillImage.fillAmount = Mathf.Lerp(AmmoFillImage.fillAmount, currenFillRatio,
                Time.deltaTime * AmmoFillMovementSharpness);

            BulletCounter.text = m_Weapon.GetCarriedPhysicalBullets().ToString();

            bool isActiveWeapon = m_Weapon == m_PlayerWeaponsManager.GetActiveWeapon();

            CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, isActiveWeapon ? 1f : UnselectedOpacity,
                Time.deltaTime * 10);
            transform.localScale = Vector3.Lerp(transform.localScale, isActiveWeapon ? Vector3.one : UnselectedScale,
                Time.deltaTime * 10);
            ControlKeysRoot.SetActive(!isActiveWeapon);

            FillBarColorChange.UpdateVisual(currenFillRatio);

            Reload.gameObject.SetActive(m_Weapon.GetCarriedPhysicalBullets() > 0 && m_Weapon.GetCurrentAmmo() == 0 && m_Weapon.IsWeaponActive);
        }

        void Destroy()
        {
            EventManager.RemoveListener<AmmoPickupEvent>(OnAmmoPickup);
        }
    }
}