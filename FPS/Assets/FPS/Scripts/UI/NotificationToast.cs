using UnityEngine;

namespace Unity.FPS.UI
{
    public class NotificationToast : MonoBehaviour
    {
        [Header("显示通知文本的文本内容")]
        public TMPro.TextMeshProUGUI TextContent;
        [Header("画布用于淡入淡出内容")]
        public CanvasGroup CanvasGroup;
        [Header("它将保持可见多久")]
        public float VisibleDuration;
        [Header("淡入淡出的持续时间")]
        public float FadeInDuration = 0.5f;
        [Header("淡出持续时间")]
        public float FadeOutDuration = 2f;

        public bool Initialized { get; private set; }
        float m_InitTime;

        public float TotalRunTime => VisibleDuration + FadeInDuration + FadeOutDuration;

        public void Initialize(string text)
        {
            TextContent.text = text;
            m_InitTime = Time.time;

            // start the fade out
            Initialized = true;
        }

        void Update()
        {
            if (Initialized)
            {
                float timeSinceInit = Time.time - m_InitTime;
                if (timeSinceInit < FadeInDuration)
                {
                    // fade in
                    CanvasGroup.alpha = timeSinceInit / FadeInDuration;
                }
                else if (timeSinceInit < FadeInDuration + VisibleDuration)
                {
                    // stay visible
                    CanvasGroup.alpha = 1f;
                }
                else if (timeSinceInit < FadeInDuration + VisibleDuration + FadeOutDuration)
                {
                    // fade out
                    CanvasGroup.alpha = 1 - (timeSinceInit - FadeInDuration - VisibleDuration) / FadeOutDuration;
                }
                else
                {
                    CanvasGroup.alpha = 0f;

                    // fade out over, destroy the object
                    Destroy(gameObject);
                }
            }
        }
        
       
    }
}

