using UnityEngine;

namespace Unity.FPS.Game
{
    // This class contains general information describing an actor (player or enemies).
    // It is mostly used for AI detection logic and determining if an actor is friend or foe
    public class Actor : MonoBehaviour
    {
        [Header("表示参与者的从属关系（或团队）。同一党派的演员彼此友好")]
        public int Affiliation;

        [Header("表示其他参与者攻击此参与者时将瞄准的点")]
        public Transform AimPoint;

        ActorsManager m_ActorsManager;

        void Start()
        {
            m_ActorsManager = GameObject.FindObjectOfType<ActorsManager>();
            DebugUtility.HandleErrorIfNullFindObject<ActorsManager, Actor>(m_ActorsManager, this);

            // Register as an actor
            if (!m_ActorsManager.Actors.Contains(this))
            {
                m_ActorsManager.Actors.Add(this);
            }
        }

        void OnDestroy()
        {
            // Unregister as an actor
            if (m_ActorsManager)
            {
                m_ActorsManager.Actors.Remove(this);
            }
        }
    }
}