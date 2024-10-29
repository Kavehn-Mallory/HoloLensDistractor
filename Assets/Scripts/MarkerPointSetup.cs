using System;
using DistractorProject.Transport;
using DistractorProject.Transport.DataContainer;
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
        private int _currentMarker;

        public Action OnMarkerSetupComplete = delegate { };
        
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
            Server.Instance.RegisterCallback<ConfirmationData>(OnPointSelectionConfirmed);
            _markerPoints[0].enabled = true;
            Server.Instance.SendNetworkMessage(new MarkerCountData
            {
                markerCount = MarkerPointCount
            });
        }

        private void ActivateMarker()
        {
            _markerPoints[_currentMarker].enabled = false;
            _currentMarker++;
            _markerPoints[_currentMarker].enabled = true;
        }

        private void OnPointSelectionConfirmed(ConfirmationData data)
        {
            if (data.confirmationNumber != _currentMarker)
            {
                //todo throw error
            }

            if (_currentMarker >= _markerPoints.Length)
            {
                EndMarkerPointSetup();
                return;
            }
            ActivateMarker();
            Server.Instance.SendNetworkMessage(new ConfirmationData
            {
                confirmationNumber = _currentMarker
            });
        }

        public void EndMarkerPointSetup()
        {
            Server.Instance.UnregisterCallback<ConfirmationData>(OnPointSelectionConfirmed);
        }
    }
}