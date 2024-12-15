
using System;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Runtime;
using TMPro;
using UnityEngine;

public class PopupProtectGirl : MonoBehaviour
{
    [SerializeField] private UIButton btnConfirm;
    [SerializeField] private TMP_Text txtConfirm;

    private int _stepCount;

    private void OnEnable()
    {
        LoadStep();
        btnConfirm.onClickEvent.AddListener(OnClick);
    }

    private void OnDisable()
    {
        btnConfirm.onClickEvent.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        
        OnIncreaseLevel();
    }

    private void LoadStep()
    {
        var apiGameInfo = FactoryApi.Get<ApiGame>().Data.Info;

        Debug.LogError("Girl ID: " + apiGameInfo.CurrentGirlId + ", lv: " + apiGameInfo.current_level_girl);
        
        var modValue = (apiGameInfo.current_level_girl + 1) % GameConsts.MAX_LEVEL_PER_CHAR;
        int stepValue = modValue == 0 ? 2 : 1;
        txtConfirm.text = $"{Localization.Get(TextId.Common_LbNextGirl)} {stepValue}/2";
    }

    private async void OnIncreaseLevel()
    {
        this.ShowProcessing();
        try
        {
            var apiGameInfo = FactoryApi.Get<ApiGame>();
            if (apiGameInfo.Data.Info.CurrentGirlId == 20009)
            {
                ControllerPopup.ShowInformation(Localization.Get(TextId.Confirm_MaxGirl));
                this.HideProcessing();
                return;
            }
            
            CheckTimeout();
            await apiGameInfo.PostIncreaseLevel();
            
            if (!apiGameInfo.Data.Info.IsNeedProtectGirl())
            {
                SpecialExtensionGame.SaveNextGirl(apiGameInfo.Data.Info.CurrentGirlId);
                this.HideProcessing();
                GetComponent<UIPopup>().Hide();
                return;
            }
            
            LoadStep();
            this.HideProcessing();
        }
        catch (Exception e)
        {
            e.ShowError();
        }
    }

    private async void CheckTimeout()
    {
        for (int i = 0; i < 10; i++)
        {
            await UniTask.Delay(1000);
            if (!FactoryApi.Get<ApiGame>().Data.Info.IsNeedProtectGirl())
                return;
        }

        this.HideProcessing();
    }
}
