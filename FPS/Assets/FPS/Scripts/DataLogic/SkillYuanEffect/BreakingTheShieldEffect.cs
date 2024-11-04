using System;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class BreakingTheShieldEffect: EffectParent
    {

        protected override void AwakeSpeed()
        {
            if (H.CurrentShield>0)
            {
                H.TakeDamage(H.CurrentShield,null);
            }
            Destroy(this);
        }

        protected override void UpdateSpeed()
        {
            
        }


        protected override void OnDestroySpeed()
        {

        }

        public override void AddData(EffectData e)
        {
            base.AddData(e);
        }


    }
    public class SecKillEffect : EffectParent
    {

        protected override void AwakeSpeed()
        {
            if (H.CurrentShield>0||H.CurrentHealth>0)
            {
                H.TakeDamage(999999,null);
            }
            Destroy(this);
        }

        protected override void UpdateSpeed()
        {
            
        }
        

        protected override void OnDestroySpeed()
        {
           
        }

        public override void AddData(EffectData e)
        {
            base.AddData(e);
        }


    }
}