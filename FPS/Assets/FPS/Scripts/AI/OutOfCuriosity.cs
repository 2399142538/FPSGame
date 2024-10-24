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
        yield return new WaitForSeconds(10);
        for (int i = 0; i < 10; i++)
        {
            GameObject e=  Instantiate(Enemy1,PatrolAll.transform.position,Quaternion.identity);
            PatrolAll.Addenemy(e.GetComponent<EnemyController>());
        }
        yield return new WaitForSeconds(10);
        for (int i = 0; i < 15; i++)
        {
            GameObject e=  Instantiate(Enemy1,PatrolAll.transform.position,Quaternion.identity);
            PatrolAll.Addenemy(e.GetComponent<EnemyController>());
        }
        yield return new WaitForSeconds(10);
        for (int i = 0; i < 20; i++)
        {
            GameObject e=  Instantiate(Enemy1,PatrolAll.transform.position,Quaternion.identity);
            PatrolAll.Addenemy(e.GetComponent<EnemyController>());
        }
    }
}