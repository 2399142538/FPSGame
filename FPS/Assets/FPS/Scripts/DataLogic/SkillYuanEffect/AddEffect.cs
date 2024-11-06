using System;
using System.Collections.Generic;
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
        /// 设置状态
        /// </summary>
        public static void SetStateController(bool isPlayer,GameObject obj,AbnormalState state)
        {
            if (isPlayer)
            {
                StateController s = obj.GetComponent<StateController>();
                if (s==null)
                {
                    s = obj.AddComponent<StateController>();
                }
                s.AddListAbnormalState(state);
            }
            else
            {
                StateController s = obj.GetComponent<StateController>();
                if (s==null)
                {
                    s = obj.AddComponent<StateController>();
                }
                s.AddListAbnormalState(state);
            }
        }
        /// <summary>
        /// 获得状态
        /// </summary>
        public static List<AbnormalState> GetStateController(bool isPlayer,GameObject obj)
        {
            StateController s = obj.GetComponent<StateController>();
            if (s==null)
            {
                return new List<AbnormalState> ();
            }

            return new List<AbnormalState>(s.ListAbnormalState);
        }
        
        /// <summary>
        /// 示例效果-50% -100% +50% +100%
        /// </summary>
        /// 硬直
        public static SlowDownEffectData Slow_50 = new SlowDownEffectData() { IsSlowDown=false,TimeStart=0.1f, TimeEnable=0.2f,TimeEnd=0.2f,AddSpeed=-0.8f,AbnormalState = AbnormalState.SlowDown};
        public static SlowDownEffectData Slow_501 = new SlowDownEffectData() { IsSlowDown=false,TimeStart=0.1f, TimeEnable=2f,TimeEnd=0.2f,AddSpeed=-0.5f,AbnormalState = AbnormalState.SlowDown};
        public static SlowDownEffectData Slow_100 = new SlowDownEffectData() { IsSlowDown=false,TimeStart=2f, TimeEnable=5,TimeEnd=2f,AddSpeed=-1f,AbnormalState = AbnormalState.SlowDown};
        public static SlowDownEffectData Slow_1000 = new SlowDownEffectData() { IsSlowDown=false,TimeStart=2f, TimeEnable=5,TimeEnd=2f,AddSpeed=-10f,AbnormalState = AbnormalState.SlowDown};
        public static SlowDownEffectData Slow_80 = new SlowDownEffectData() { IsSlowDown=false,TimeStart=2f, TimeEnable=5,TimeEnd=2f,AddSpeed=-0.8f,AbnormalState = AbnormalState.SlowDown};
        public static SlowDownEffectData Add_50 = new SlowDownEffectData() { IsSlowDown=true,TimeStart=0.1f, TimeEnable=2,TimeEnd=0.2f,AddSpeed=0.5f,AbnormalState = AbnormalState.Accelerate};
        public static SlowDownEffectData Add_100 = new SlowDownEffectData() { IsSlowDown=true,TimeStart=2f, TimeEnable=2,TimeEnd=2f,AddSpeed=1f,AbnormalState = AbnormalState.Accelerate};
        public static SlowDownEffectData Add_1000 = new SlowDownEffectData() { IsSlowDown=true,TimeStart=2f, TimeEnable=2,TimeEnd=2f,AddSpeed=10f,AbnormalState = AbnormalState.Accelerate};

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
        
        public static HpDownEffectData AddHP_1000 = new HpDownEffectData() { IsAdd=true,isUseSheild=true, TimeDuration=5,TimeInterval=2f,AddHp=20,Id=11111,AbnormalState = AbnormalState.ContinuousReply};
        public static HpDownEffectData ReduceHP_1000 = new HpDownEffectData() { IsAdd=false,isUseSheild=true, TimeDuration=5,TimeInterval=2f,AddHp=20,Id=222,AbnormalState = AbnormalState.ContinuousHurt};
        
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
        /// 施加破盾isKill=false/秒杀效果isKill=true   
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="e"></param>
        public void BreakingTheShieldEffect(GameObject tr,bool isKill)
        {
            if (isKill)
            {
                BreakingTheShieldEffect<SecKillEffect>(tr,null);
            }
            else
            {
                BreakingTheShieldEffect<BreakingTheShieldEffect>(tr,null);
            }
        }
        
        /// <summary>
        /// 施加感电效果
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="e"></param>
        public void AddGanDianEffect(GameObject tr,GanDianEffectData e)
        {
            BreakingTheShieldEffect<GanDianEffect>(tr,e);
        }
        
        public void BreakingTheShieldEffect<T>(GameObject tr,EffectData e) where T : EffectParent
        {
            bool isPlayer=tr.GetComponent<PlayerCharacterController>() != null ;
            T a;
            if (isPlayer)
            {
                a = tr.GetComponent<T>();
                if (a == null)
                {
                    a = tr.AddComponent<T>();
                    a.AddData(e);
                }
            }
            else
            {
                Health b = tr.GetComponentInParent<Health>();
                if (b!=null)
                {
                    a = b.GetComponent<T>();
                    if (a == null)
                    {
                        a = b.gameObject.AddComponent<T>();
                        a.AddData(e);
                    }
                }

            }
        }

    }
}