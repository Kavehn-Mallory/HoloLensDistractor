using System.Collections.Generic;
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
        }

        [ContextMenu("Add Position")]
        public void AddPlacementPosition()
        {
            var position = _mainCameraTransform.position + _mainCameraTransform.forward;
        
            _distractorPlacementPositions.Add(position);
        }

        public Vector3 GetRandomPlacementPosition()
        {
            return _distractorPlacementPositions[Random.Range(0, _distractorPlacementPositions.Count)];
        }
    
    }
}