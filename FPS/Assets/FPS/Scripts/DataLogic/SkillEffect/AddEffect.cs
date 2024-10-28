using System;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class AddEffect:MonoBehaviour
    {
        public static AddEffect instance;

        private void Awake()
        {
            instance = this;
        }
        /// <summary>
        /// 示例效果-50% -100% +50% +100%
        /// </summary>
        public static SlowDownEffectData Slow_50 = new SlowDownEffectData() { IsSlowDown=false,TimeStart=0.1f, TimeEnable=2,TimeEnd=0.2f,AddSpeed=-0.5f};
        public static SlowDownEffectData Slow_100 = new SlowDownEffectData() { IsSlowDown=false,TimeStart=2f, TimeEnable=2,TimeEnd=2f,AddSpeed=-1f};
        public static SlowDownEffectData Add_50 = new SlowDownEffectData() { IsSlowDown=true,TimeStart=0.1f, TimeEnable=2,TimeEnd=0.2f,AddSpeed=0.5f};
        public static SlowDownEffectData Add_100 = new SlowDownEffectData() { IsSlowDown=true,TimeStart=2f, TimeEnable=2,TimeEnd=2f,AddSpeed=1f};
        
        public void SlowDownEffect(GameObject tr,SlowDownEffectData e)
        {
            SlowDownEffect a = tr.GetComponent<SlowDownEffect>();
            if (a!=null)
            {
                a.AddData(e);
            }
            else
            {
                a= tr.AddComponent<SlowDownEffect>();
                a.AddData(e);
            }
        }
        
    }
}