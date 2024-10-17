using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class JetpackCounter : MonoBehaviour
    {
        [Header("代表喷气背包燃料的图像组件")]
        public Image JetpackFillImage;

        [Header("包含喷气式飞机整个用户界面的画布组")]
        public CanvasGroup MainCanvasGroup;

        [Header("用于在空或满时设置颜色动画的组件")]
        public FillBarColorChange FillBarColorChange;

        Jetpack m_Jetpack;

        void Awake()
        {
            m_Jetpack = FindObjectOfType<Jetpack>();
            DebugUtility.HandleErrorIfNullFindObject<Jetpack, JetpackCounter>(m_Jetpack, this);

            FillBarColorChange.Initialize(1f, 0f);
        }

        void Update()
        {
            MainCanvasGroup.gameObject.SetActive(m_Jetpack.IsJetpackUnlocked);

            if (m_Jetpack.IsJetpackUnlocked)
            {
                JetpackFillImage.fillAmount = m_Jetpack.CurrentFillRatio;
                FillBarColorChange.UpdateVisual(m_Jetpack.CurrentFillRatio);
            }
        }
    }
}