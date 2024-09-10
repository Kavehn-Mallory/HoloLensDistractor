using MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class DistractorComponent : MonoBehaviour
{
    public int distractorIndex = -1;

    public DistractorTaskManager Manager { get; set; }
    
    private void Awake()
    {
        //replace the pressable button with normal buttons and replace the input logic 
        GetComponent<Button>().onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        Manager.OnButtonClicked(distractorIndex);
    }
}