using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.UI
{
    public class CompassElement : MonoBehaviour
    {
        [Header("指南针上这个元素的标记")]
        public CompassMarker CompassMarkerPrefab;

        [Header("Text override for the marker, if it's a direction")]
        public string TextDirection;

        Compass m_Compass;

        void Awake()
        {
            m_Compass = FindObjectOfType<Compass>();
            DebugUtility.HandleErrorIfNullFindObject<Compass, CompassElement>(m_Compass, this);

            var markerInstance = Instantiate(CompassMarkerPrefab);

            markerInstance.Initialize(this, TextDirection);
            m_Compass.RegisterCompassElement(transform, markerInstance);
        }

        void OnDestroy()
        {
            m_Compass.UnregisterCompassElement(transform);
        }
    }
}