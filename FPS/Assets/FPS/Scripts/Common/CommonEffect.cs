using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonEffect : MonoBehaviour
{
    public static CommonEffect instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public GameObject 引雷标记;
    public GameObject 斩杀粒子;
    public GameObject 闪电链粒子;
    public GameObject 闪电粒子;
    public GameObject 雷阵粒子;
    public GameObject 附带闪电粒子;
    
}
