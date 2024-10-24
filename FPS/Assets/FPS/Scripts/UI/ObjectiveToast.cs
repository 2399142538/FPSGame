﻿using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class ObjectiveToast : MonoBehaviour
    {
        [Header("References显示标题的文本内容")]
        public TMPro.TextMeshProUGUI TitleTextContent;

        [Header("将显示描述的文本内容")]
        public TMPro.TextMeshProUGUI DescriptionTextContent;

        [Header("显示计数器的文本内容")]
        public TMPro.TextMeshProUGUI CounterTextContent;

        [Header("Rect将显示描述")]
        public RectTransform SubTitleRect;

        [Header("画布用于淡入淡出内容")]
        public CanvasGroup CanvasGroup;

        [Header("包含目标的布局组")]
        public HorizontalOrVerticalLayoutGroup LayoutGroup;

        [Header("Transitions移动完成前的延迟")]
        public float CompletionDelay;

        [Header("淡入淡出的持续时间")] public float FadeInDuration = 0.5f;
        [Header("淡出持续时间")] public float FadeOutDuration = 2f;

        [Header("声音初始化时播放的声音")]
        public AudioClip InitSound;

        [Header("完成后将播放的声音")]
        public AudioClip CompletedSound;

        [Header("运动")] [Header("在屏幕中移动所需的时间")]
        public float MoveInDuration = 0.5f;

        [Header("随时间推移在x中移动和定位的动画曲线")]
        public AnimationCurve MoveInCurve;

        [Header("移出屏幕所需的时间")]
        public float MoveOutDuration = 2f;

        [Header("移出的动画曲线，随时间在x中的位置")]
        public AnimationCurve MoveOutCurve;

        float m_StartFadeTime;
        bool m_IsFadingIn;
        bool m_IsFadingOut;
        bool m_IsMovingIn;
        bool m_IsMovingOut;
        AudioSource m_AudioSource;
        RectTransform m_RectTransform;

        public void Initialize(string titleText, string descText, string counterText, bool isOptionnal, float delay)
        {
            // set the description for the objective, and forces the content size fitter to be recalculated
            Canvas.ForceUpdateCanvases();

            TitleTextContent.text = titleText;
            DescriptionTextContent.text = descText;
            CounterTextContent.text = counterText;

            if (GetComponent<RectTransform>())
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            }

            m_StartFadeTime = Time.time + delay;
            // start the fade in
            m_IsFadingIn = true;
            m_IsMovingIn = true;
        }

        public void Complete()
        {
            m_StartFadeTime = Time.time + CompletionDelay;
            m_IsFadingIn = false;
            m_IsMovingIn = false;

            // if a sound was set, play it
            PlaySound(CompletedSound);

            // start the fade out
            m_IsFadingOut = true;
            m_IsMovingOut = true;
        }

        void Update()
        {
            float timeSinceFadeStarted = Time.time - m_StartFadeTime;

            SubTitleRect.gameObject.SetActive(!string.IsNullOrEmpty(DescriptionTextContent.text));

            if (m_IsFadingIn && !m_IsFadingOut)
            {
                // fade in
                if (timeSinceFadeStarted < FadeInDuration)
                {
                    // calculate alpha ratio
                    CanvasGroup.alpha = timeSinceFadeStarted / FadeInDuration;
                }
                else
                {
                    CanvasGroup.alpha = 1f;
                    // end the fade in
                    m_IsFadingIn = false;

                    PlaySound(InitSound);
                }
            }

            if (m_IsMovingIn && !m_IsMovingOut)
            {
                // move in
                if (timeSinceFadeStarted < MoveInDuration)
                {
                    LayoutGroup.padding.left = (int) MoveInCurve.Evaluate(timeSinceFadeStarted / MoveInDuration);

                    if (GetComponent<RectTransform>())
                    {
                        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
                    }
                }
                else
                {
                    // making sure the position is exact
                    LayoutGroup.padding.left = 0;

                    if (GetComponent<RectTransform>())
                    {
                        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
                    }

                    m_IsMovingIn = false;
                }

            }

            if (m_IsFadingOut)
            {
                // fade out
                if (timeSinceFadeStarted < FadeOutDuration)
                {
                    // calculate alpha ratio
                    CanvasGroup.alpha = 1 - (timeSinceFadeStarted) / FadeOutDuration;
                }
                else
                {
                    CanvasGroup.alpha = 0f;

                    // end the fade out, then destroy the object
                    m_IsFadingOut = false;
                    Destroy(gameObject);
                }
            }

            if (m_IsMovingOut)
            {
                // move out
                if (timeSinceFadeStarted < MoveOutDuration)
                {
                    LayoutGroup.padding.left = (int) MoveOutCurve.Evaluate(timeSinceFadeStarted / MoveOutDuration);

                    if (GetComponent<RectTransform>())
                    {
                        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
                    }
                }
                else
                {
                    m_IsMovingOut = false;
                }
            }
        }

        void PlaySound(AudioClip sound)
        {
            if (!sound)
                return;

            if (!m_AudioSource)
            {
                m_AudioSource = gameObject.AddComponent<AudioSource>();
                m_AudioSource.outputAudioMixerGroup = AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.HUDObjective);
            }

            m_AudioSource.PlayOneShot(sound);
        }
    }
}