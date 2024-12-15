using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindow : MonoBehaviour
{
    [SerializeField] private Image imgBg;
    [SerializeField] private UIToggleGroup toggleGroup;
    [SerializeField] private Color[] arrColor;

    private void OnEnable()
    {
        toggleGroup.OnToggleTriggeredCallback.AddListener(OnToggle);
    }

    private void OnDisable()
    {
        toggleGroup.OnToggleTriggeredCallback.RemoveListener(OnToggle);
    }
    
    private void OnToggle(UIToggle toggle)
    {
        var index = toggleGroup.lastToggleOnIndex;
        imgBg.color = index == 0 ? arrColor[0] : arrColor[1];
    }
}
