using UnityEngine;

namespace Unity.FPS.Game
{
    public class ConstantRotation : MonoBehaviour
    {
        [Header("每秒旋转角度")] public float RotatingSpeed = 360f;

        void Update()
        {
            // Handle rotating
            transform.Rotate(Vector3.up, RotatingSpeed * Time.deltaTime, Space.Self);
        }
    }
}