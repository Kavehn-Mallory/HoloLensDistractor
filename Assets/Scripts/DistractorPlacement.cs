using System;
using System.Collections.Generic;
using DistractorProject.Core;
using DistractorProject.Transport;
using DistractorProject.Transport.DataContainer;
using MixedReality.Toolkit.Input;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DistractorProject
{
    public class DistractorPlacement : MonoBehaviour
    {

        [SerializeField] private Camera mainCamera;
        [SerializeField] private FuzzyGazeInteractor gazeInteractor;
    
        private readonly List<Vector3> _distractorPlacementPositions = new();
        private Transform _mainCameraTransform;

        private int _currentMarkerPoint;
        private int _markerPointCount;
        private bool _acceptInput;

        private void Awake()
        {
            if (!mainCamera)
            {
                mainCamera = Camera.main;
            }
            _mainCameraTransform = mainCamera?.transform;
            if (!_mainCameraTransform)
            {
                Debug.LogError($"No main camera found. Please either add one to {nameof(DistractorPlacement)} or tag an active camera with \"MainCamera\"");
                enabled = false;
            }
            
            //todo register to client 
        }

        private void Start()
        {
            Client.Instance.RegisterCallback<MarkerCountData>(StartMarkerPlacement);
        }

        private void StartMarkerPlacement(MarkerCountData obj)
        {
            throw new NotImplementedException();
        }


        [ContextMenu("Add Position")]
        public void AddPlacementPosition()
        {
            if (!_acceptInput)
            {
                return;
            }
            var position = _mainCameraTransform.position + _mainCameraTransform.forward;
        
            _distractorPlacementPositions.Add(position);
            Client.Instance.SendNetworkMessage(new ConfirmationData
            {
                confirmationNumber = _currentMarkerPoint
            });
        }

        public Vector3 GetRandomPlacementPosition()
        {
            return _distractorPlacementPositions[Random.Range(0, _distractorPlacementPositions.Count)];
        }


        public void OnConfirmationDataReceived(ConfirmationData confirmationData)
        {
            _currentMarkerPoint = confirmationData.confirmationNumber;
        }

        public void OnMarkerCountDataReceived(MarkerCountData markerCountData)
        {
            _markerPointCount = markerCountData.markerCount;
        }
    
    }
}