using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    public class DamageArea : MonoBehaviour
    {
        [Header("弹丸击中某物时的损伤区域")]
        public float AreaOfEffectDistance = 5f;

        [Header("影响区域距离上的损伤乘数")]
        public AnimationCurve DamageRatioOverDistance;

        [Header("效果半径区域的颜色")]
        public Color AreaOfEffectColor = Color.red * 0.5f;

        public void InflictDamageInArea(float damage, Vector3 center, LayerMask layers,
            QueryTriggerInteraction interaction, GameObject owner)
        {
            Dictionary<Health, Damageable> uniqueDamagedHealths = new Dictionary<Health, Damageable>();

            // Create a collection of unique health components that would be damaged in the area of effect (in order to avoid damaging a same entity multiple times)
            Collider[] affectedColliders = Physics.OverlapSphere(center, AreaOfEffectDistance, layers, interaction);
            foreach (var coll in affectedColliders)
            {
                Damageable damageable = coll.GetComponent<Damageable>();
                if (damageable)
                {
                    Health health = damageable.GetComponentInParent<Health>();
                    if (health && !uniqueDamagedHealths.ContainsKey(health))
                    {
                        uniqueDamagedHealths.Add(health, damageable);
                    }
                }
            }

            // Apply damages with distance falloff
            foreach (Damageable uniqueDamageable in uniqueDamagedHealths.Values)
            {
                float distance = Vector3.Distance(uniqueDamageable.transform.position, transform.position);
                uniqueDamageable.InflictDamage(
                    damage * DamageRatioOverDistance.Evaluate(distance / AreaOfEffectDistance), true, owner);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = AreaOfEffectColor;
            Gizmos.DrawSphere(transform.position, AreaOfEffectDistance);
        }
    }
}