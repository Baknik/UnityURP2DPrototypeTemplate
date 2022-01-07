using MVCEngine;
using MVCEngine.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FPSCounterView : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;
    private Performance _performance;

    private void Awake()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        _performance = Engine.Instance.GraphicsQualityController.GetPerformanceModel();
    }
    
    private void Start()
    {
        UpdateText();
    }

    private void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        _textMeshProUGUI.text = $"{_performance.FPS.ToString("F2")} FPS";
    }
}