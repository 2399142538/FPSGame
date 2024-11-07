using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;


class LightningZhenEffect : MonoBehaviour
{
    public float r;
    public float LeiZhenMax;
    public float KillCount;
    public bool IsKill;
    public void Init()
    {
        InvokeRepeating("InflictDamageInArea",0.5f,0.5f);
    }
    
    public void InflictDamageInArea()
    {
            
        // Create a collection of unique health components that would be damaged in the area of effect (in order to avoid damaging a same entity multiple times)
        Collider[] affectedColliders = Physics.OverlapSphere(transform.position, r, -1, QueryTriggerInteraction.Collide);
        foreach (var coll in affectedColliders)
        {
            Damageable damageable = coll.GetComponent<Damageable>();
            if (damageable)
            {
                Health health = damageable.GetComponentInParent<Health>();
                if (health )
                {
                    if (health.GetComponent<PlayerCharacterController>())
                    {
          
                    }
                    else
                    {
                        //收到
                        AddEffect.instance.AddGanDianEffect(health.gameObject,new GanDianEffectData(){IsKill = IsKill,KillCount=KillCount});
                    }
                }
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red * 0.5f;;
        Gizmos.DrawSphere(transform.position, 5);
    }
}

/// <summary>
    /// 闪电子弹效果
    /// </summary>
    public class LightningBulletsEffect: EffectParent
    {
        protected override void UpdateSpeed()
        {
            
        }

        protected async override void AwakeSpeed()
        {
            GameObject a = Instantiate(CommonEffect.instance.引雷标记,E.Down.transform);
            Destroy(a,1f);
            await Task.Delay(1000);
            GameObject b = Instantiate(CommonEffect.instance.闪电粒子,E.Middle.transform);
            Destroy(b,1000f);
            new HpDownEffectData()
            {
                IsAdd = false, isUseSheild = true, TimeDuration = 5, TimeInterval = 2f, AddHp = 20, Id = 222,
                AbnormalState = AbnormalState.ContinuousHurt
            };
    
            float r= GameData.instance.LightningBullets(2);        //半球
            int d=(int) GameData.instance.LightningBullets(1);        //半球
            InflictDamageInArea(d, transform.position, r);
            //if ( GameData.instance.LightningBullets(7)>0)
            {
                GameObject c = Instantiate(CommonEffect.instance.雷阵粒子,E.Down.transform.position,Quaternion.identity);
                Destroy(c,5f);
                LightningZhenEffect l= c.AddComponent<LightningZhenEffect>();
                float s= GameData.instance.LightningBullets(2);
                l.transform.localScale = new Vector3(s,s,s);
                l.KillCount = 80;// GameData.instance.LightningBullets(6);
                l.IsKill = true;//  GameData.instance.LightningBullets(5)>0;     
                l.r=  GameData.instance.LightningBullets(2);     
                l.Init();
            }
            await Task.Delay(5000);
            Destroy(this);
        }

        public void InflictDamageInArea(int damage, Vector3 center, float r)
        {
            
            // Create a collection of unique health components that would be damaged in the area of effect (in order to avoid damaging a same entity multiple times)
            Collider[] affectedColliders = Physics.OverlapSphere(center, r, -1, QueryTriggerInteraction.Collide);
            foreach (var coll in affectedColliders)
            {
                Damageable damageable = coll.GetComponent<Damageable>();
                if (damageable)
                {
                    Health health = damageable.GetComponentInParent<Health>();
                    if (health )
                    {
                        if (health.GetComponent<PlayerCharacterController>())
                        {
                            health.TakeDamage(1, null);
                        }
                        else
                        {
                            //收到
                            health.TakeDamage(damage, null);
                        }
                    }
                }
            }
        }
        
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red * 0.5f;;
            Gizmos.DrawSphere(transform.position, 5);
        }
        
        protected override void OnDestroySpeed()
        {
           
        }
    }
