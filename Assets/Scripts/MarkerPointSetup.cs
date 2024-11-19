using System;
using DistractorProject.Transport;
using DistractorProject.Transport.DataContainer;
using UnityEngine;
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
            markerPointCanvas.gameObject.SetActive(false);
            _markerPoints = CreateMarkerPoints(marker, zones);

        }

        public int MarkerPointCount => _markerPoints.Length;

        private Image[] CreateMarkerPoints(Image image, Vector2Int markerZones)
        {
            var result = new Image[markerZones.x * markerZones.y];

            var xStep = Screen.width / markerZones.x;
            var yStep = Screen.height / markerZones.y;

            for (int y = 0; y < markerZones.y; y++)
            {
                for (int x = 0; x < markerZones.x; x++)
                {
                    var markerInstance = Instantiate(marker, new Vector3(0.5f * xStep + xStep * x, 0.5f * yStep + yStep * y), Quaternion.identity, markerPointCanvas.transform);
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
            markerPointCanvas.gameObject.SetActive(true);
            Debug.Log("Starting Marker-Setup");
            Server.Instance.TransmitNetworkMessage(new MarkerCountData
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

            if (_currentMarker >= _markerPoints.Length - 1)
            {
                EndMarkerPointSetup();
                return;
            }
            Debug.Log("Activating next marker");
            ActivateMarker();
            Server.Instance.TransmitNetworkMessage(new ConfirmationData
            {
                confirmationNumber = _currentMarker
            });
        }

        public void EndMarkerPointSetup()
        {
            _markerPoints[^1].enabled = false;
            markerPointCanvas.gameObject.SetActive(false);
            Server.Instance.UnregisterCallback<ConfirmationData>(OnPointSelectionConfirmed);
            //todo remove this and just sign up for the next event in the client. Might need to end the event for the placement system tho
            Server.Instance.TransmitNetworkMessage(new MarkerSetupEndData());
            OnMarkerSetupComplete.Invoke();
        }
    }
}