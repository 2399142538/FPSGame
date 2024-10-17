using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class FillBarColorChange : MonoBehaviour
    {
        [Header("前景前景图像")]
        public Image ForegroundImage;

        [Header("默认前景颜色")] public Color DefaultForegroundColor;

        [Header("满时闪烁前景颜色")]
        public Color FlashForegroundColorFull;

        [Header("背景背景图片")]
        public Image BackgroundImage;

        [Header("空白时背景颜色闪烁")]
        public Color DefaultBackgroundColor;

        [Header("颜色变化的锐度")]
        public Color FlashBackgroundColorEmpty;

        [Header("Values价值考虑充分")]
        public float FullValue = 1f;

        [Header("要考虑的值为空")] public float EmptyValue = 0f;

        [Header("颜色变化的锐度")]
        public float ColorChangeSharpness = 5f;

        float m_PreviousValue;

        public void Initialize(float fullValueRatio, float emptyValueRatio)
        {
            FullValue = fullValueRatio;
            EmptyValue = emptyValueRatio;

            m_PreviousValue = fullValueRatio;
        }

        public void UpdateVisual(float currentRatio)
        {
            if (currentRatio == FullValue && currentRatio != m_PreviousValue)
            {
                ForegroundImage.color = FlashForegroundColorFull;
            }
            else if (currentRatio < EmptyValue)
            {
                BackgroundImage.color = FlashBackgroundColorEmpty;
            }
            else
            {
                ForegroundImage.color = Color.Lerp(ForegroundImage.color, DefaultForegroundColor,
                    Time.deltaTime * ColorChangeSharpness);
                BackgroundImage.color = Color.Lerp(BackgroundImage.color, DefaultBackgroundColor,
                    Time.deltaTime * ColorChangeSharpness);
            }

            m_PreviousValue = currentRatio;
        }
    }
}