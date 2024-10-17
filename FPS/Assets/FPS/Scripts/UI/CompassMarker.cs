using Unity.FPS.AI;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class CompassMarker : MonoBehaviour
    {
        [Header("主标记图像")] public Image MainImage;

        [Header("标记的画布组")]
        public CanvasGroup CanvasGroup;

        [Header("敌方元素")] [Header("标记的默认颜色")]
        public Color DefaultColor;

        [Header("标记的替代颜色")]
        public Color AltColor;

        [Header("方向元素")] [Header("将此标记用作磁性方向")]
        public bool IsDirection;

        [Header("方向的文本内容")]
        public TMPro.TextMeshProUGUI TextContent;

        EnemyController m_EnemyController;

        public void Initialize(CompassElement compassElement, string textDirection)
        {
            if (IsDirection && TextContent)
            {
                TextContent.text = textDirection;
            }
            else
            {
                m_EnemyController = compassElement.transform.GetComponent<EnemyController>();

                if (m_EnemyController)
                {
                    m_EnemyController.onDetectedTarget += DetectTarget;
                    m_EnemyController.onLostTarget += LostTarget;

                    LostTarget();
                }
            }
        }

        public void DetectTarget()
        {
            MainImage.color = AltColor;
        }

        public void LostTarget()
        {
            MainImage.color = DefaultColor;
        }
    }
}