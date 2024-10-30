using System;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    public class Health : MonoBehaviour
    {
        [Header("最大健康值")] public int MaxHealth = 10;
        [Header("最大护盾值")] public int MaxShield = 10;
        
        [Header("关键健康小插曲开始出现的健康比率")]
        public float CriticalHealthRatio = 0.3f;
        /// <summary>
        /// 护盾受伤回调
        /// </summary>
        public UnityAction<float, GameObject> OnShieldDamaged;
        /// <summary>
        /// 护盾被打破回调
        /// </summary>
        public UnityAction OnShieldDie;
        
        public UnityAction<float, GameObject> OnDamaged;
        public UnityAction<float> OnHealed;
        
        public UnityAction OnDie;

        [Header("受伤后几秒内不攻击 开始恢复数据")]
        public float ShieldIntervalTime=5;
        
        [Header("开始恢复后一次恢复间隔")]
        public float ShieldRecovery=3;
        
        [Header(" 开始恢复后一次恢复多少")]
        public int ShieldRecoveryCount=3;

        /// <summary>
        /// 当前血量
        /// </summary>
        public int CurrentHealth;

        /// <summary>
        /// 当前护盾
        /// </summary>
        public int CurrentShield;
        /// <summary>
        /// 是否无敌
        /// </summary>
        public bool Invincible { get; set; }
        //是否可以拾取体力
        public bool CanPickup() => CurrentHealth < MaxHealth;

        /// <summary>
        /// 获得比例
        /// </summary>
        /// <returns></returns>
        public float GetShieldRatio() => (float) CurrentShield / MaxShield;
        /// <summary>
        /// 获得比例
        /// </summary>
        /// <returns></returns>
        public float GetRatio() => (float)CurrentHealth / MaxHealth;
        /// <summary>
        /// 快死亡效果
        /// </summary>
        /// <returns></returns>
        public bool IsCritical() => GetRatio() <= CriticalHealthRatio;
        /// <summary>
        /// 是否拥有护盾
        /// </summary>
        /// <returns></returns>
        public bool IsHasShield() => MaxShield>0;
        /// <summary>
        /// 是否拥有护盾
        /// </summary>
        /// <returns></returns>
        public bool BreakingTheShield=false;

        bool m_IsDead;

        public void Init()
        {
            CurrentHealth = MaxHealth;
            CurrentShield = MaxShield;
        }

        /// <summary>
        /// 治愈逻辑
        /// </summary>
        /// <param name="healAmount"></param>
        public void Heal(int healAmount)
        {
            float healthBefore = CurrentHealth;
            CurrentHealth += healAmount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

            // call OnHeal action
            float trueHealAmount = CurrentHealth - healthBefore;
            if (trueHealAmount > 0f)
            {
                OnHealed?.Invoke(trueHealAmount);
            }
        }


        private void Update()
        {
            RecoveryShield();
        }

        private DateTime DamageTime = DateTime.MaxValue;
        private DateTime RecoveryTime = DateTime.MinValue;
/// <summary>
/// 恢复护盾
/// </summary>
        void RecoveryShield()
        {
            if (BreakingTheShield|| CurrentShield>=MaxShield-0.01f)
            {
                return;
            }

            if ((DateTime.Now - DamageTime).TotalSeconds > ShieldIntervalTime)
            {
                if ((DateTime.Now - RecoveryTime).TotalSeconds > ShieldRecovery)
                {
                    RecoveryTime = DateTime.Now;
                    CurrentShield += ShieldRecoveryCount;
                }
  
            }
            

        }
        
        /// <summary>
        /// 造成伤害
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="damageSource"></param>
        public void TakeDamage(int damage, GameObject damageSource)
        {
            if (Invincible)
                return;

            float ShieldBefore = CurrentShield;
            if (ShieldBefore>0)
            {
                CurrentShield -= damage;
                //护盾没破
                if (CurrentShield>0)
                {
                    DamageTime=DateTime.Now;
                    RecoveryTime=DateTime.Now;
                    CurrentShield = Mathf.Clamp(CurrentShield, 0, MaxHealth);
                    // call OnDamage action
                    float trueDamageAmount = ShieldBefore - CurrentShield;
                    if (trueDamageAmount > 0f)
                    {
                        
                        OnShieldDamaged?.Invoke(trueDamageAmount, damageSource);
                    }
                }
                else
                {
                    
                    int ShieldBefore2 = CurrentShield;
                    CurrentShield = 0;
                    OnShieldDie?.Invoke();
                    BreakingTheShield = true;
                    TakeDamage(ShieldBefore2,damageSource);
                }


            }
            else
            {
                float healthBefore = CurrentHealth;
                CurrentHealth -= damage;
                CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

                // call OnDamage action
                float trueDamageAmount = healthBefore - CurrentHealth;
                if (trueDamageAmount > 0f)
                {
                    OnDamaged?.Invoke(trueDamageAmount, damageSource);
                }

                HandleDeath();
            }
       
        }

        /// <summary>
        /// 死亡
        /// </summary>
        public void Kill()
        {
            CurrentHealth = 0;
            // call OnDamage action
            OnDamaged?.Invoke(MaxHealth, null);

            HandleDeath();
        }

        /// <summary>
        /// 死亡
        /// </summary>
        void HandleDeath()
        {
            if (m_IsDead)
                return;

            // call OnDie action
            if (CurrentHealth <= 0f)
            {
                m_IsDead = true;
                OnDie?.Invoke();
            }
        }
    }
}