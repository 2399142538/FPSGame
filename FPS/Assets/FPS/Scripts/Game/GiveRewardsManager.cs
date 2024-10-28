using UnityEngine;

namespace Unity.FPS.Game
{
    public class GiveRewardsManager:MonoBehaviour
    {
        public static GiveRewardsManager instance;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
    }
}