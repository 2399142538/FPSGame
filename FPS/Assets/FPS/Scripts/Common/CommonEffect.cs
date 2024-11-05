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
    }

    public GameObject 引雷标记;
    public GameObject 斩杀粒子;
    public GameObject 闪电链粒子;
    public GameObject 雷;
    public GameObject 雷阵粒子;
    
}
