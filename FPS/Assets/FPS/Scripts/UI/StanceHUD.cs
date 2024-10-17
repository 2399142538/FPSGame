using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{

    public class StanceHUD : MonoBehaviour
    {
        [Header("姿态精灵的图像组件")]
        public Image StanceImage;

        [Header("站立时显示的雪碧")]
        public Sprite StandingSprite;

        [Header("蹲下时显示的雪碧")]
        public Sprite CrouchingSprite;

        [Header("跑步时显示的雪碧")]
        public Sprite RuningSprite;

        void Start()
        {
            PlayerCharacterController character = FindObjectOfType<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, StanceHUD>(character, this);
            character.OnStanceChanged += OnStanceChanged;

            OnStanceChanged(character.IsCrouching);
        }

  
        void OnStanceChanged(PlayerStanceStage crouched)
        {
            switch (crouched)
            {
                case PlayerStanceStage.Standing:
                    StanceImage.sprite = StandingSprite;
                    break;
                case PlayerStanceStage.Crouching:
                    StanceImage.sprite = CrouchingSprite;
                    break;
                case PlayerStanceStage.Runing:
                    StanceImage.sprite = RuningSprite;
                    break;
            }
        }
    }
}