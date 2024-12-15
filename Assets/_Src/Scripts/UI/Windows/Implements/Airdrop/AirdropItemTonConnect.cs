using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using UnityEngine;

public class AirdropItemTonConnect : MonoBehaviour
{
    [SerializeField] private TMP_Text txtContent;
    [SerializeField] private GameObject objConnect;
    [SerializeField] private GameObject objConnected;
    [SerializeField] private UIButton btnConnect;

    private void Awake()
    {
        btnConnect.onClickEvent.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        btnConnect.onClickEvent.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        
    }
}
