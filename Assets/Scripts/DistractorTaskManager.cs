using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class DistractorTaskManager : MonoBehaviour
{
    
    [SerializeField] private Canvas canvas;
    [SerializeField] private DistractorComponent label;
    [SerializeField] private int numberOfDistractors = 5;
    [SerializeField] private float distanceFromCenter = 1f;
    [SerializeField] private float distanceFromCamera = 1f;
    [SerializeField] private float canvasWidth = 1f;
    [Tooltip("Determines the distance of the distractors from the center based on the horizontal viewing angle")]
    [SerializeField] private float targetDistractorAngleFromCenter = 5f;
    [Tooltip("Determines the distance of the peripheral distractor from the center based on the horizontal viewing angle")]
    [SerializeField] private float peripheralDistractorAngleFromCenter = 7f;
    [Tooltip("Determines the size of the distractors based on the horizontal viewing angle")]
    [SerializeField] private float targetDistractorViewAngle = 2f;
    [SerializeField] private Vector2 peripheralDistractorPosition = new Vector2(-300, 0);
    [SerializeField] private float peripheralDistractorFontSize = 48f;
    
    [SerializeField] private DistractorShapeGroup[] distractorShapes;
    [Tooltip("Set to true if the target should never be the same target in two consecutiveTrials")]
    [SerializeField] private bool changeTargetAfterEveryTrial;
    

    private int _currentGroup;

    private float _adjustedFontSize = 1f;
    

    private string[][] _distractorShapes;
    private string[][] _targetShapes;
    private TMP_Text[] _distractors;
    private TMP_Text _peripheralDistractor;
    private int _targetElementIndex;

    private void Awake()
    {

        CalculateOffsets();
        
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
        _peripheralDistractor.rectTransform.anchoredPosition = peripheralDistractorPosition;
        _peripheralDistractor.gameObject.name = "Peripheral Distractor";
        _peripheralDistractor.fontSize = peripheralDistractorFontSize;
        
        
        var mainCamera = Camera.main;
        
        if (!mainCamera)
        {
            Debug.LogError("No main camera found. Is required for correct positioning of target", this);
            enabled = false;
            return;
        }

        
        var mainCameraTransform = mainCamera.transform;
        canvas.transform.SetLocalPositionAndRotation(Vector3.forward * distanceFromCamera, mainCamera.transform.rotation);

        
        

        var angle = 360f / (numberOfDistractors + 1);

        var currentAngle = 0f;
        foreach (var distractor in _distractors)
        {
            distractor.PlaceLabelsAtPosition(mainCameraTransform, distanceFromCenter, currentAngle);
            currentAngle += angle;
            distractor.fontSize = _adjustedFontSize;
        }

        _peripheralDistractor.fontSize = 2 * _adjustedFontSize;
        
        StartNextTrial();
    }

    private void CalculateOffsets()
    {
        //all units in meters?
        var targetOffset = CalculateActualSize(distanceFromCamera, targetDistractorAngleFromCenter);
        var targetSize = CalculateActualSize(distanceFromCamera, targetDistractorViewAngle);
        var peripheralOffset = CalculateActualSize(distanceFromCamera, peripheralDistractorAngleFromCenter);
        
        
        //lets assume the canvas is 1 meter wide
        var dimensions = canvas.pixelRect;

        Debug.Log(canvas.pixelRect.height);
        var scaleFactor = canvasWidth / dimensions.width;
        
        canvas.GetComponent<RectTransform>().localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        
        Debug.Log($"Coordinates: {targetOffset / scaleFactor}, {targetSize / scaleFactor}, {peripheralOffset / scaleFactor}");

        distanceFromCenter = targetOffset / scaleFactor;
        peripheralDistractorPosition = new Vector2(-peripheralOffset / scaleFactor, 0);
        var targetSizeInPixel = targetSize / scaleFactor;
        label.GetComponent<RectTransform>().sizeDelta = new Vector2(targetSizeInPixel, targetSizeInPixel);

        _adjustedFontSize = targetSizeInPixel;

    }
    


    private float CalculateActualSize(float r, float alpha)
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

    private void OnIncorrectButtonClicked()
    {
        Debug.Log("Incorrect button pressed");
    }

    private void OnCorrectButtonClicked()
    {
        Debug.Log("Correct button pressed");
    }

    public void TestButtonClick()
    {
        Debug.Log("Button was clicked");
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
