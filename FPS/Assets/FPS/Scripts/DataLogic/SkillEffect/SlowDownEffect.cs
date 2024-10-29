using System;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.AI;
using Unity.FPS.Gameplay;
using UnityEngine;



public struct SlowDownEffectData
{
    /// <summary>
    /// //加速or减速 true 加速
    /// </summary>
    public bool IsSlowDown;
    //所有的减速效果分为三段
    /// <summary>
    /// //缓慢减速时间  为0为瞬时减速默认0.1f
    /// </summary>
    public float TimeStart;
    /// <summary>
    /// 减速 到最小速度 持续时间默认0.1f
    /// </summary>
    public float TimeEnable; 
    /// <summary>
    /// 速度上升时间 默认0.1f
    /// </summary>
    public float TimeEnd;
    /// <summary>
    /// 减的速度
    /// </summary>
    public float AddSpeed;
    
    
    /// <summary>
    /// 特殊标识符
    /// </summary>
    public int Id;
    public float SpeedTemp;
    public float TimeStartTemp;
    public float TimeEnableTemp; 
    public float TimeEndTemp;
    public float TempChaZhi;

    public void Init()
    {
        TimeStartTemp = TimeStart;
        TimeEnableTemp = TimeEnable;
        TimeEndTemp = TimeEnd;
        SpeedTemp = 1+AddSpeed;
        SpeedTemp = Mathf.Max(0, SpeedTemp);
    }
}



/// <summary>
/// 速度处理，
/// </summary>
public class SlowDownEffect : MonoBehaviour
{
    
    public static int id = 0;

    public static int GetId()
    {
        id += 1;
        return id;
    }
    // Start is called before the first frame update
    //区分玩家还是怪物
    //减速到终点的时间：类似2秒内减速到0
    //减速到终点的持续效果
    
    //减速效果类似减少30%
    public bool IsPlayer = false;

    private List<SlowDownEffectData> SlowDownEffectDatas = new List<SlowDownEffectData>();
    void Update()
    {
        UpdateSpeed();
    }

    private void OnDestroy()
    {
        if (IsPlayer)
        {
            gameObject.GetComponent<PlayerCharacterController>().MaxSpeedCoefficient=1;
        }
        else
        {
            gameObject.GetComponent<EnemyController>().MaxSpeedCoefficient=1;
        }
    }

    public void AddData(SlowDownEffectData e)
    {
        e.Id=GetId();
        e.Init();
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
            if (gameObject.GetComponent<PlayerCharacterController>()==null&&gameObject.GetComponent<EnemyController>()==null)
            {
                return;
                
            }
            if (IsPlayer)
            {
                speed = gameObject.GetComponent<PlayerCharacterController>().MaxSpeedCoefficient;
                addSpeed = gameObject.GetComponent<PlayerCharacterController>().MaxSpeedCoefficient2;
            }
            else
            {
                speed = gameObject.GetComponent<EnemyController>().MaxSpeedCoefficient;
                addSpeed = gameObject.GetComponent<EnemyController>().MaxSpeedCoefficient2;
            }
            
 
            for (int i = SlowDownEffectDatas.Count-1; i >=0; i--)
            {
                SlowDownEffectData a = SlowDownEffectDatas[i];

     
                if (a.TimeStartTemp>0)
                {
                    a.TimeStartTemp -= 0.1f;
                    if (speed>a.SpeedTemp&& !a.IsSlowDown)
                    {
                        speed += a.AddSpeed / (a.TimeStart / 0.1f);
                        SlowDownEffectDatas[i] = a;
                    }
                    else if(addSpeed<a.SpeedTemp&& a.IsSlowDown)
                    {
                        addSpeed += a.AddSpeed / (a.TimeStart / 0.1f);
                        SlowDownEffectDatas[i] = a;
                    }
                    else
                    {
                        a.TempChaZhi +=  a.AddSpeed /(a.TimeStart / 0.1f);
                        SlowDownEffectDatas[i] = a;
                    }
                }
                else
                {
                    if (a.TimeEnableTemp > 0 && (!a.IsSlowDown))
                    {
                        a.TimeEnableTemp -= 0.1f;
                        if (speed > a.SpeedTemp)
                        {
                            speed = a.SpeedTemp;

                        }

                        SlowDownEffectDatas[i] = a;
                    }
                    else if (a.TimeEnableTemp > 0 && (a.IsSlowDown) )
                    {
                        a.TimeEnableTemp -= 0.1f;
                        if (addSpeed<a.SpeedTemp)
                        {
                            addSpeed = a.SpeedTemp;
                        }
                        SlowDownEffectDatas[i] = a;
                    }
                    else
                    {
                        if (a.TimeEndTemp > 0)
                        {
                            a.TimeEndTemp -= 0.1f;
                            if (speed < 1&& (!a.IsSlowDown))
                            {
                                speed -= a.AddSpeed / (a.TimeEnd / 0.1f);
                            }else if (addSpeed > 1&& (a.IsSlowDown))
                            {
                                addSpeed -= a.AddSpeed / (a.TimeEnd / 0.1f);
                            }
                            SlowDownEffectDatas[i] = a;
                        }
                        else
                        {
                            SlowDownEffectDatas.RemoveAt(i);
                        }
                    }
                }
            }
            if (IsPlayer)
            {
                gameObject.GetComponent<PlayerCharacterController>().MaxSpeedCoefficient= Mathf.Round(speed * 100) / 100;
                gameObject.GetComponent<PlayerCharacterController>().MaxSpeedCoefficient2=Mathf.Round(addSpeed * 100) / 100;
            }
            else
            {
                gameObject.GetComponent<EnemyController>().MaxSpeedCoefficient= Mathf.Round(speed * 100) / 100;
                gameObject.GetComponent<EnemyController>().MaxSpeedCoefficient2= Mathf.Round(addSpeed * 100) / 100;
            }
        }
        else
        {
            Destroy(this);
        }
    }
}
