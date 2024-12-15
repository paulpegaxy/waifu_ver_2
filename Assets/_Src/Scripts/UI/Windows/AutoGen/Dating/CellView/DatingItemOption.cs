
using System;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using TMPro;
using UnityEngine;

public class DatingItemOption : MonoBehaviour
{
    [SerializeField] private TMP_Text txtOption;
    [SerializeField] private UIButton btnChoose;

    private DataItemMessageOption _data;

    public static Action<DataItemMessageOption> OnChooseOption;

    public void SetData(DataItemMessageOption data)
    {
        _data = data;
        txtOption.text = data.optionMessage.InsertLineBreaksAfterWords(GameConsts.MAX_WORD_PER_LINE);
        
#if UNITY_EDITOR
        txtOption.text += " - " + data.nextNodeOptionId.SetHighlightStringOrange();
#endif
    }

    private void OnEnable()
    {
        btnChoose.onClickEvent.AddListener(OnChoose);
    }

    private void OnDisable()
    {
        btnChoose.onClickEvent.RemoveListener(OnChoose);
    }

    private void OnChoose()
    {
        // Debug.LogError("Choose option: " + _data.optionMessage + ", next: " + _data.nextNodeId);
        OnChooseOption?.Invoke(_data);
    }
}
