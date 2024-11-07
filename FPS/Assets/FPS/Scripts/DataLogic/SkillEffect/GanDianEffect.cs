using System;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;


public class GanDianEffectData:EffectData
{
    public bool IsKill;
    public float KillCount;
    public float TotalSeconds=10;
    
}



/// <summary>
/// 感电Effect
/// </summary>
public class GanDianEffect : EffectParent
{
    private bool isKill;
    private float killCount;
    private float TotalSeconds;
    public override void AddData(EffectData e)
    {
        if (e is GanDianEffectData e1)
        {
            isKill = e1.IsKill;
            killCount = e1.KillCount;
            TotalSeconds = e1.TotalSeconds;
        }
        
    }


    DateTime _time=DateTime.Now;
    
    protected override void UpdateSpeed()
    {
     
        if (TotalSeconds<=0)
        {
            Destroy(this);
            return;
        }
        if ((DateTime.Now-_time).TotalSeconds>2f&&TotalSeconds>0)
        {
            _time = DateTime.Now;
            TotalSeconds -= 2;
        }
        else
        {
            return;
        }

    
        if (isKill&&H.CurrentHealth<H.MaxHealth*(killCount/100))
        {
            AddEffect.instance.BreakingTheShieldEffect(gameObject,true);
            GameObject c = Instantiate(CommonEffect.instance.斩杀粒子,E.Middle.transform);
            Destroy(c,0.5f);
        }
        AddEffect.instance.SlowDownEffect(gameObject,new SlowDownEffectData() {Id = -1,IsSlowDown=false,TimeStart=0.1f, TimeEnable=0.5f,TimeEnd=0.1f,AddSpeed=-0.9f,AbnormalState = AbnormalState.SlowDown});
        GameObject a = Instantiate(CommonEffect.instance.附带闪电粒子,E.Middle.transform);
        Destroy(a,0.7f);
    }

    protected override void AwakeSpeed()
    {
        //效果
    }

    protected override void OnDestroySpeed()
    {
      
    }
}
