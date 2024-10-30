using System;
using System.Collections.Generic;
using Unity.FPS.AI;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{

    public struct HpDownEffectData
    {
        /// <summary>
        /// 加血，掉血 true 加
        /// </summary>
        public bool IsAdd;
        
        public bool isUseSheild;

        //所有的减速效果分为三段
        /// <summary>
        /// 总持续时间
        /// </summary>
        public float TimeDuration;

        /// <summary>
        /// 间隔多久一次
        /// </summary>
        public float TimeInterval;

        /// <summary>
        /// 一次减的血
        /// </summary>
        public int AddHp;

        
        /// <summary>
        /// 表示给予掉血的类型
        /// </summary>
        public int Id;
        
        public float TimeDurationTemp;
        public float TimeIntervalTemp;
        
        
        
        public float TempChaZhi;


        
    }

    public class AddBloodOrSheildEffect : MonoBehaviour
    {
        public static int id = 0;
        public static int GetId()
        {
            id += 1;
            return id;
        }
        
          public bool IsPlayer = false;

    private List<HpDownEffectData> SlowDownEffectDatas = new List<HpDownEffectData>();
    private Health H;
    private void Awake()
    {
         H = gameObject.GetComponent<Health>();
    }

    void Update()
    {
        UpdateSpeed();
    }

    private void OnDestroy()
    {


    }

    public void AddData(HpDownEffectData e)
    {
        for (int i = 0; i < SlowDownEffectDatas.Count; i++)
        {
            if (SlowDownEffectDatas[i].Id==e.Id)
            {
                HpDownEffectData d = SlowDownEffectDatas[i];
                d.TimeDuration = e.TimeDuration;
                SlowDownEffectDatas[i]=d;
                return;
            }
        }
        SlowDownEffectDatas.Add(e);
    }
    // Update is called once per frame
    
    DateTime _time=DateTime.Now;
    void UpdateSpeed()
    {
        if ((DateTime.Now-_time).TotalSeconds>0.1f)
        {
            _time = DateTime.Now;
        }
        else
        {
            return;
        }
        if (SlowDownEffectDatas.Count>0)
        {
            float speed=1;
            float addSpeed=1;
    
            if (H==null)
            {
                Destroy(this);
                return;
            }
    
            
 
            for (int i = SlowDownEffectDatas.Count-1; i >=0; i--)
            {
                HpDownEffectData a = SlowDownEffectDatas[i];
                
                if (a.TimeDurationTemp<a.TimeDuration)
                {
                    a.TimeDurationTemp += 0.1f;
                    SlowDownEffectDatas[i] = a;
                    if (a.TimeIntervalTemp<a.TimeInterval)
                    {
                        a.TimeIntervalTemp+= 0.1f;
                        SlowDownEffectDatas[i] = a;
                    }
                    else
                    {
                        a.TimeIntervalTemp = 0;
                        if (a.IsAdd)
                        {
                            H.Heal(a.AddHp);
                        }
                        else
                        {
                            H.TakeDamage(a.AddHp,null);
                        }
                        SlowDownEffectDatas[i] = a;
                    }

                }
                else
                {
                    SlowDownEffectDatas.RemoveAt(i);
                }
            }

        }
        else
        {
            Destroy(this);
        }
    }
        
    }
}