using UnityEngine;

namespace Unity.FPS.AI
{
    // Component used to override values on start from the NavmeshAgent component in order to change
    // how the agent  is moving
    public class NavigationModule : MonoBehaviour
    {
        [Header("参数敌人移动的最大速度（每秒世界单位）。")]
        public float MoveSpeed = 0f;

        [Header("敌人旋转的最大速度（度每秒）。")]
        public float AngularSpeed = 0f;

        [Header("达到最大速度的加速度（世界单位每秒平方).")]
        public float Acceleration = 0f;
    }
}