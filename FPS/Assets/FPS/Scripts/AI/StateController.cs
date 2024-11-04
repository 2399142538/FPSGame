using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 异常状态
/// </summary>
public enum AbnormalState
{
    /// <summary>
    /// 异常状态
    /// </summary>
    AbnormalState,

    /// <summary>
    /// 减速
    /// </summary>
    SlowDown,

    /// <summary>
    /// 加速
    /// </summary>

    Accelerate,

    /// <summary>
    /// 眩晕
    /// </summary>

    Vertigo,

    /// <summary>
    /// 回复
    /// </summary>
    ContinuousReply,
    
    /// <summary>
    /// 伤害
    /// </summary>
    ContinuousHurt,
    
    
    /// <summary>
    /// 感电
    /// </summary>
    ElectricInduction,
    /// <summary>
    /// 大燃烧
    /// </summary>
    BBurning,
    /// <summary>
    /// 小燃烧
    /// </summary>
    Sburning,
}

public class StateController : MonoBehaviour
{
    public List<AbnormalState> ListAbnormalState = new List<AbnormalState>();

    public void AddListAbnormalState(AbnormalState state)
    {
        ListAbnormalState.Add(state);
    }

    
}
