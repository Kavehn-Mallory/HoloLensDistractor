using System.Collections.Generic;
using UnityEngine;

public class DistractorPlacement : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    
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