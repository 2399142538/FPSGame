using System;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class BreakingTheShieldEffect: MonoBehaviour
    {
        private Health H;
        private void Awake()
        {
            H = gameObject.GetComponent<Health>();
            if (H.CurrentShield>0)
            {
                H.TakeDamage(H.CurrentShield,null);
            }
            Destroy(this);
        }
        
    }
}