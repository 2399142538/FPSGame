using System;
using Unity.FPS.AI;
using Unity.FPS.Game;
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
        /// 硬直
        public static SlowDownEffectData Slow_50 = new SlowDownEffectData() { IsSlowDown=false,TimeStart=0.1f, TimeEnable=0.2f,TimeEnd=0.2f,AddSpeed=-0.8f};
        public static SlowDownEffectData Slow_501 = new SlowDownEffectData() { IsSlowDown=false,TimeStart=0.1f, TimeEnable=2f,TimeEnd=0.2f,AddSpeed=-0.5f};
        
        public static SlowDownEffectData Slow_100 = new SlowDownEffectData() { IsSlowDown=false,TimeStart=2f, TimeEnable=5,TimeEnd=2f,AddSpeed=-1f};
        public static SlowDownEffectData Slow_1000 = new SlowDownEffectData() { IsSlowDown=false,TimeStart=2f, TimeEnable=5,TimeEnd=2f,AddSpeed=-10f};
        public static SlowDownEffectData Slow_80 = new SlowDownEffectData() { IsSlowDown=false,TimeStart=2f, TimeEnable=5,TimeEnd=2f,AddSpeed=-0.8f};
        public static SlowDownEffectData Add_50 = new SlowDownEffectData() { IsSlowDown=true,TimeStart=0.1f, TimeEnable=2,TimeEnd=0.2f,AddSpeed=0.5f};
        public static SlowDownEffectData Add_100 = new SlowDownEffectData() { IsSlowDown=true,TimeStart=2f, TimeEnable=2,TimeEnd=2f,AddSpeed=1f};
        public static SlowDownEffectData Add_1000 = new SlowDownEffectData() { IsSlowDown=true,TimeStart=2f, TimeEnable=2,TimeEnd=2f,AddSpeed=10f};

        public void SlowDownEffect(GameObject tr, SlowDownEffectData e)
        {
            bool isPlayer=tr.GetComponent<PlayerCharacterController>() != null ;

            SlowDownEffect a;
            if (isPlayer)
            {
                 a = tr.GetComponent<SlowDownEffect>();
                 if (a == null)
                 {
                     a = tr.AddComponent<SlowDownEffect>();
             
                 }
                 a.IsPlayer = true;
                 a.AddData(e);
            }
            else
            {
                EnemyController b = tr.GetComponentInParent<EnemyController>();
                if (b!=null)
                {
                    a = b.GetComponent<SlowDownEffect>();
                    if (a == null)
                    {
                        a = b.gameObject.AddComponent<SlowDownEffect>();
                    }
                    a.IsPlayer = false;
                    a.AddData(e);
                }

            }
        }
        
        public static HpDownEffectData AddHP_1000 = new HpDownEffectData() { IsAdd=true,isUseSheild=true, TimeDuration=5,TimeInterval=2f,AddHp=20,Id=11111};
        public static HpDownEffectData ReduceHP_1000 = new HpDownEffectData() { IsAdd=false,isUseSheild=true, TimeDuration=5,TimeInterval=2f,AddHp=20,Id=222};
        
        /// <summary>
        /// 施加持续伤害效果
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="e"></param>
        public void HpDownEffect(GameObject tr, HpDownEffectData e)
        {
            bool isPlayer=tr.GetComponent<PlayerCharacterController>() != null ;
            AddBloodOrSheildEffect a;
            if (isPlayer)
            {
                 a = tr.GetComponent<AddBloodOrSheildEffect>();
                 if (a == null)
                 {
                     a = tr.AddComponent<AddBloodOrSheildEffect>();
             
                 }
                 a.IsPlayer = true;
                 a.AddData(e);
            }
            else
            {
                Health b = tr.GetComponentInParent<Health>();
                if (b!=null)
                {
                    a = b.GetComponent<AddBloodOrSheildEffect>();
                    if (a == null)
                    {
                        a = b.gameObject.AddComponent<AddBloodOrSheildEffect>();
                    }
                    a.IsPlayer = false;
                    a.AddData(e);
                }

            }
        }        
        /// <summary>
        /// 施加破盾效果
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="e"></param>
        public void BreakingTheShieldEffect(GameObject tr)
        {
            bool isPlayer=tr.GetComponent<PlayerCharacterController>() != null ;
            BreakingTheShieldEffect a;
            if (isPlayer)
            {
                 a = tr.GetComponent<BreakingTheShieldEffect>();
                 if (a == null)
                 {
                     a = tr.AddComponent<BreakingTheShieldEffect>();
             
                 }
            }
            else
            {
                Health b = tr.GetComponentInParent<Health>();
                if (b!=null)
                {
                    a = b.GetComponent<BreakingTheShieldEffect>();
                    if (a == null)
                    {
                        a = b.gameObject.AddComponent<BreakingTheShieldEffect>();
                    }
                }

            }
        }

    }
}