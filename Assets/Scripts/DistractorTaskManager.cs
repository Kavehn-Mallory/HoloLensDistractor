using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.UI;
using Random = UnityEngine.Random;


public class DistractorTaskManager : MonoBehaviour
{
    
    [SerializeField] private Canvas canvas;
    [SerializeField] private DistractorComponent label;
    [SerializeField] private int numberOfDistractors = 5;
    
    [Header("Distractor Placement Settings")]
    [SerializeField] private float defaultDistanceFromCamera = 1f;
    [SerializeField] private float canvasWidth = 1f;
    [Tooltip("Determines the distance of the distractors from the center based on the horizontal viewing angle")]
    [SerializeField] private float targetDistractorAngleFromCenter = 5f;
    [Tooltip("Determines the distance of the peripheral distractor from the center based on the horizontal viewing angle")]
    [SerializeField] private float peripheralDistractorAngleFromCenter = 7f;
    [Tooltip("Determines the size of the distractors based on the horizontal viewing angle")]
    [SerializeField] private float targetDistractorViewAngle = 2f;
    
    [Header("Distractor Trial Settings")]
    [SerializeField] private DistractorShapeGroup[] distractorShapes;
    [Tooltip("Set to true if the target should never be the same target in two consecutiveTrials")]
    [SerializeField] private bool changeTargetAfterEveryTrial;
    
        
    private int _currentGroup;

    private string[][] _distractorShapes;
    private string[][] _targetShapes;
    private TMP_Text[] _distractors;
    private TMP_Text _peripheralDistractor;
    private int _targetElementIndex;
    private Camera _mainCamera;
    private DistractorComponent _selectedDistractor;


    private void Start()
    {
        _currentGroup = 0;
        _targetElementIndex = -1;
        _distractorShapes = new string[distractorShapes.Length][];
        _targetShapes = new string[distractorShapes.Length][];
        for (var i = 0; i < distractorShapes.Length; i++)
        {
            var shapeGroup = distractorShapes[i];
            _distractorShapes[i] = shapeGroup.distractorLetters.Split(',');
            _targetShapes[i] = shapeGroup.targetLetters.Split(',');
        }

        _distractors = new TMP_Text[numberOfDistractors + 1];
        for (int i = 0; i < numberOfDistractors + 1; i++)
        {
            var labelInstance = Instantiate(label, canvas.transform, false);
            labelInstance.gameObject.name = "Distractor";
            labelInstance.distractorIndex = i;
            labelInstance.Manager = this;
            _distractors[i] = labelInstance.GetComponent<TMP_Text>();
        }

        var peripheralDistractor = Instantiate(label, canvas.transform, false);
        peripheralDistractor.Manager = this;
        _peripheralDistractor = peripheralDistractor.GetComponent<TMP_Text>();
        _peripheralDistractor.gameObject.name = "Peripheral Distractor";
        
        
        _mainCamera = Camera.main;
        
        if (!_mainCamera)
        {
            Debug.LogError("No main camera found. Is required for correct positioning of target", this);
            enabled = false;
            return;
        }

        RepositionCanvas(_mainCamera.transform.position + Vector3.forward * defaultDistanceFromCamera);
        StartNextTrial();
    }
    

    private void RepositionCanvas(Vector3 position)
    {
        
        var distanceFromCamera = math.distance(position, _mainCamera.transform.position);
        var targetOffset = CalculateActualSize(distanceFromCamera, targetDistractorAngleFromCenter);
        var targetSize = CalculateActualSize(distanceFromCamera, targetDistractorViewAngle);
        var peripheralOffset = CalculateActualSize(distanceFromCamera, peripheralDistractorAngleFromCenter);
        
        var dimensions = canvas.pixelRect;
        
        var scaleFactor = canvasWidth / dimensions.width;
        
        canvas.GetComponent<RectTransform>().localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        
        var distanceFromCenter = targetOffset / scaleFactor;
        var peripheralDistractorPosition = new Vector2(-peripheralOffset / scaleFactor, 0);
        var targetSizeInPixel = targetSize / scaleFactor;

        canvas.transform.SetPositionAndRotation(position, _mainCamera.transform.rotation);

        var angle = 360f / (numberOfDistractors + 1);

        var currentAngle = 0f;
        foreach (var distractor in _distractors)
        {
            distractor.GetComponent<DistractorComponent>().UpdateDistractorSize(targetSizeInPixel);
            distractor.PlaceLabelsAtPosition(_mainCamera.transform, distanceFromCenter, currentAngle);
            currentAngle += angle;
        }

        _peripheralDistractor.GetComponent<DistractorComponent>().UpdateDistractorSize(targetSizeInPixel * 2f);
        _peripheralDistractor.rectTransform.anchoredPosition = peripheralDistractorPosition;
    }
    


    private static float CalculateActualSize(float r, float alpha)
    {
        var radians = math.radians(alpha);
        return math.abs(2f * r * math.tan((radians / 2f)));
    }

    [ContextMenu("Next Trial")]
    public void StartNextTrial()
    {
        var length = _distractorShapes[_currentGroup].Length;
        foreach (var distractor in _distractors)
        {

            distractor.text = _distractorShapes[_currentGroup][Random.Range(0, length)];
        }

        if (changeTargetAfterEveryTrial && _targetElementIndex >= 0)
        {
            _targetElementIndex += Random.Range(1, _distractors.Length);
            _targetElementIndex %= _distractors.Length;
        }
        else
        {
            _targetElementIndex = Random.Range(0, _distractors.Length);
        }
        
        _distractors[_targetElementIndex].text =
            _targetShapes[_currentGroup][Random.Range(0, _targetShapes[_currentGroup].Length)];
        
        _peripheralDistractor.text = _targetShapes[_currentGroup][Random.Range(0, _targetShapes[_currentGroup].Length)];
    }

    [ContextMenu("Next Group")]
    public void SelectNextGroup()
    {
        _currentGroup++;
        if (_currentGroup < _distractorShapes.Length)
        {
            _targetElementIndex = -1;
            StartNextTrial();
            return;
        }
        Debug.Log("Trials completed");
    }

    public void OnButtonClicked(int id)
    {
        if (_targetElementIndex == id && id >= 0)
        {
            OnCorrectButtonClicked();
        }
        else
        {
            OnIncorrectButtonClicked();
        }
        StartNextTrial();
        
    }
    
    public void OnHoverEnter(UIHoverEventArgs args)
    {
        _selectedDistractor = args.uiObject?.GetComponent<DistractorComponent>();
    }

    public void OnHoverExit()
    {
        _selectedDistractor = null;
    }

    [ContextMenu("Select button")]
    public void OnSelectionConfirmed()
    {
        if (_selectedDistractor)
        {
            Debug.Log(_selectedDistractor.distractorIndex);
            OnButtonClicked(_selectedDistractor.distractorIndex);
            return;
        }
        OnButtonClicked(-1);
    }

    private void OnIncorrectButtonClicked()
    {
        Debug.Log("Incorrect button pressed");
    }

    private void OnCorrectButtonClicked()
    {
        Debug.Log("Correct button pressed");
    }
    

    [Serializable]
    public struct DistractorShapeGroup
    {

        public string groupName;
        [Tooltip("Separate options with a ','")]
        public string distractorLetters;

        [Tooltip("Separate options with a ','")]
        public string targetLetters;
    }
    
    
}
