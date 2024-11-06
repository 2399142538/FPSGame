using System;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;


public class GanDianEffectData:EffectData
{
    public bool IsKill;
}



/// <summary>
/// 感电Effect
/// </summary>
public class GanDianEffect : EffectParent
{
    private bool isKill;
    public override void AddData(EffectData e)
    {
        if (e is GanDianEffectData e1)
        {
            isKill = e1.IsKill;
        }
        
    }


    DateTime _time=DateTime.Now;
    protected override void UpdateSpeed()
    {
        if ((DateTime.Now-_time).TotalSeconds>2f)
        {
            _time = DateTime.Now;
        }
        else
        {
            return;
        }
        if (isKill&&H.CurrentHealth<H.MaxHealth/0.3f)
        {
            AddEffect.instance.BreakingTheShieldEffect(gameObject,true);
        }
        AddEffect.instance.SlowDownEffect(gameObject,new SlowDownEffectData() {Id = -1,IsSlowDown=false,TimeStart=0.1f, TimeEnable=0.1f,TimeEnd=0.5f,AddSpeed=-0.9f,AbnormalState = AbnormalState.SlowDown});
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
