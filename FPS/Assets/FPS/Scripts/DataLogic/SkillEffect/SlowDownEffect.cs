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
    void Start()
    {
        InvokeRepeating("UpdateSpeed", 0.0f, 0.2f);
    }

    public void AddData(SlowDownEffectData e)
    {
        e.Id=GetId();
        SlowDownEffectDatas.Add(e);
    }
    // Update is called once per frame
    void UpdateSpeed()
    {

        if (SlowDownEffectDatas.Count>0)
        {
            float speed=1;
            if (IsPlayer)
            {
                speed = gameObject.GetComponent<PlayerCharacterController>().MaxSpeedCoefficient;
            }
            else
            {
                speed = gameObject.GetComponent<EnemyController>().MaxSpeedCoefficient;
            }
            for (int i = SlowDownEffectDatas.Count-1; i >=0; i--)
            {
                SlowDownEffectData a = SlowDownEffectDatas[i];
                if (a.TimeStart>0)
                {
                    a.TimeStart -= 0.1f;
                    speed += a.AddSpeed / (a.TimeStart / 0.1f);
                    a.SpeedTemp = speed;
                }
                else
                {
                    if (a.TimeEnable > 0)
                    {
                        a.TimeEnable -= 0.2f;
                        if (speed>a.SpeedTemp)
                        {
                            speed = a.SpeedTemp;
                        }
                    }
                    else
                    {
                        if (a.TimeEnd > 0)
                        {
                            a.TimeEnd -= 0.1f;
                            speed += a.AddSpeed / (a.TimeEnd / 0.1f);
                            a.SpeedTemp = speed;
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
                gameObject.GetComponent<PlayerCharacterController>().MaxSpeedCoefficient=speed;
            }
            else
            {
                gameObject.GetComponent<EnemyController>().MaxSpeedCoefficient=speed;
            }
        }
        else
        {
            Destroy(this);
        }
    }
}
