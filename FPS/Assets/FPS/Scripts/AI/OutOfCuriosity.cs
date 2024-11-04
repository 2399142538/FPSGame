using System;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.AI;
using UnityEngine;

public class OutOfCuriosity : MonoBehaviour
{
    [Header("路线")] 
    public PatrolPath PatrolAll;
    [Header("生成的怪")] 
    public GameObject Enemy1;

    private void Start()
    {
        StartCoroutine(OutOfCuriosityLogic());
    }

    IEnumerator OutOfCuriosityLogic()
    {

        List<int> list = GameDataLevelData.instance.GetLevelConfig().WaveCount;
        for (int i = 0; i <list .Count; i++)
        {      
            yield return new WaitForSeconds(10);
            for (int j = 0; j < list[i]; j++)
            {
                GameObject e=  Instantiate(Enemy1,PatrolAll.transform.position,Quaternion.identity);
                PatrolAll.Addenemy(e.GetComponent<EnemyController>());
            }

        }
  


    }
}