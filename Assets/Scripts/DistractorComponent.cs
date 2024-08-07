using System;
using MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TMP_Text), typeof(PressableButton))]
public class DistractorComponent : MonoBehaviour
{
    public int distractorIndex = -1;

    public DistractorTaskManager Manager { get; set; }
    
    private void Awake()
    {
        GetComponent<PressableButton>().OnClicked.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        Manager.OnButtonClicked(distractorIndex);
    }
}