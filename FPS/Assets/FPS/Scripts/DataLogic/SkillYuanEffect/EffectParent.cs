using System;
using System.Collections.Generic;
using Unity.FPS.AI;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    
    public class EffectData
    {
        public AbnormalState AbnormalState;
    }


    public abstract class EffectParent:MonoBehaviour
    {
        
        protected PlayerCharacterController P;
        protected EnemyController E;
        protected Health H;
        public bool IsPlayer = false;
        private void Awake()
        {
            P = gameObject.GetComponent<PlayerCharacterController>();
            E = gameObject.GetComponent<EnemyController>();
            H = gameObject.GetComponent<Health>();
            AwakeSpeed();
        }

        private void Update()
        {
            UpdateSpeed();
        }

        private void OnDestroy()
        {
            OnDestroySpeed();
        }

        protected abstract void UpdateSpeed();
        protected abstract void AwakeSpeed();
        protected abstract void OnDestroySpeed();

        public virtual void AddData(EffectData e)
        {
            AddState(e.AbnormalState);
        }
        
        public List<AbnormalState> ListAbnormalState = new List<AbnormalState>();
        //增加状态
        protected void AddState(AbnormalState s)
        {
            ListAbnormalState.Add(s);
        }
        //结束状态
        protected  void RemoveState(AbnormalState s)
        {
            ListAbnormalState.Remove(s);
        }
        
        protected static int id = 0;
        protected static int GetId()
        {
            id += 1;
            return id;
        }
    }
}