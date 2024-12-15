using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;

[RequireComponent(typeof(UIButton))]
public class ButtonTonConnect : MonoBehaviour
{
    private UIButton _btn;

    private void Awake()
    {
        _btn = GetComponent<UIButton>();
    }

    private void OnEnable()
    {
        _btn.onClickEvent.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _btn.onClickEvent.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        
    }
}
