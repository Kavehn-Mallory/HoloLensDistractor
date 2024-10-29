using System;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace DistractorProject
{
    public class MarkerPointSetup : MonoBehaviour
    {
        [SerializeField] private Canvas markerPointCanvas;
        [SerializeField] private Vector2Int zones;
        [SerializeField] private Image marker;

        private Image[] _markerPoints = Array.Empty<Image>();
        private void Awake()
        {
            markerPointCanvas.enabled = false;
            _markerPoints = CreateMarkerPoints(marker, zones);

        }

        public int MarkerPointCount => _markerPoints.Length;

        private Image[] CreateMarkerPoints(Image image, Vector2Int markerZones)
        {
            var result = new Image[markerZones.x * markerZones.y];

            for (int y = 0; y < markerZones.y; y++)
            {
                for (int x = 0; x < markerZones.x; x++)
                {
                    var markerInstance = Instantiate(marker, new Vector3(), Quaternion.identity, markerPointCanvas.transform);
                    result[y * markerZones.x + x] = markerInstance;
                    markerInstance.enabled = false;
                }
            }

            return result;
        }

        public void StartMarkerPointSetup()
        {
            
        }

        public void EndMarkerPointSetup()
        {
            
        }
    }
}