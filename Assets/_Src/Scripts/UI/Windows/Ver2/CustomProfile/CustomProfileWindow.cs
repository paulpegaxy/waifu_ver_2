using System;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Game.Extensions;
using Game.Runtime;
using Game.UI;
using Newtonsoft.Json;
using Sirenix.Utilities;
using Template.Defines;
using UnityEngine;
using UnityEngine.UI;

public class CustomProfileWindow : MonoBehaviour
{
    [SerializeField] private CustomProfilePanelEnterName panelEnterName;
    [SerializeField] private ACustomProfilePanel[] arrPanels;
    [SerializeField] private Image sliderProgress;

    private string _userName;
    private int _maxProgress;
    private int _currentProgress;
    
    private void OnEnable()
    {
        _maxProgress = 5;
        _currentProgress = 1;
        UpdateProgress();
        TypeFilterPanelCustomProfile type = TypeFilterPanelCustomProfile.name;
        var checkType = this.GetEventData<TypeGameEvent, TypeFilterPanelCustomProfile>(TypeGameEvent.EditProfile, true);
        
        // Debug.LogError("checkType: " + checkType);
        // ReSharper disable once RedundantCheckBeforeAssignment
        if (type != checkType)
        {
            type = checkType;
        }

        OnCheckShow(type);
        CustomProfilePanelEnterName.OnSendName += OnSendName;
        ACustomProfilePanel.OnBack += OnBack;
        ACustomProfilePanel.OnNext += OnNext;
    }
    
    private void OnDisable()
    {
        CustomProfilePanelEnterName.OnSendName -= OnSendName;
        ACustomProfilePanel.OnBack -= OnBack;
        ACustomProfilePanel.OnNext -= OnNext;
    }

    private void OnCheckShow(TypeFilterPanelCustomProfile type)
    {
        // Debug.LogError("type: " + type);
        if (type==TypeFilterPanelCustomProfile.name)
        {
            panelEnterName.gameObject.SetActive(true);
            arrPanels.ForEach(x => x.gameObject.SetActive(false));
        }
        else
        {
            panelEnterName.gameObject.SetActive(false);
            ShowPanel(type);
        }
    }
    
    private void OnSendName(string name)
    {
        _userName = name;
        _currentProgress++;
        UpdateProgress();
        panelEnterName.gameObject.SetActive(false);
        arrPanels[0].gameObject.SetActive(true);
    }

    private void OnNext(TypeFilterPanelCustomProfile current)
    {
        _currentProgress++;
        UpdateProgress();
        switch (current)
        {
            case TypeFilterPanelCustomProfile.interested_in:
                ShowPanel(TypeFilterPanelCustomProfile.zodiac);
                break;
            case TypeFilterPanelCustomProfile.zodiac:
                ShowPanel(TypeFilterPanelCustomProfile.genres);
                break;
            case TypeFilterPanelCustomProfile.genres:
                ShowPanel(TypeFilterPanelCustomProfile.ava_index);
                break;
            case TypeFilterPanelCustomProfile.ava_index:
                OnPostCustomProfile();
                break;
        }
    }

    private void OnBack(TypeFilterPanelCustomProfile current)
    {
        _currentProgress--;
        UpdateProgress();
        switch (current)
        {
            case TypeFilterPanelCustomProfile.interested_in:
                arrPanels.ForEach(x => x.gameObject.SetActive(false));
                panelEnterName.gameObject.SetActive(true);
                break;
            case TypeFilterPanelCustomProfile.zodiac:
                ShowPanel(TypeFilterPanelCustomProfile.interested_in);
                break;
            case TypeFilterPanelCustomProfile.genres:
                ShowPanel(TypeFilterPanelCustomProfile.zodiac);
                break;
            case TypeFilterPanelCustomProfile.ava_index:
                ShowPanel(TypeFilterPanelCustomProfile.genres);
                break;
        }
    }
    
    private void ShowPanel(TypeFilterPanelCustomProfile type)
    {
        for (var i = 0; i < arrPanels.Length; i++)
        {
            var ele = arrPanels[i];
            if (ele.TypePanel == type)
            {
                ele.gameObject.SetActive(true);
            }
            else
            {
                ele.gameObject.SetActive(false);
            }
        }
    }

    private async void OnPostCustomProfile()
    {
        Dictionary<string, string> dictProfile = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(_userName))
        {
            dictProfile.Add("name", _userName);
        }
        for (var i = 0; i < arrPanels.Length; i++)
        {
            var ele = arrPanels[i];
            if (!string.IsNullOrEmpty(ele.KeyProfile))
            {
                dictProfile.Add(ele.TypePanel.ToString(), ele.KeyProfile);
            }
        }

        // Debug.Log("data: " + JsonConvert.SerializeObject(dictProfile));
        this.ShowProcessing();
        try
        {
            var status = await FactoryApi.Get<ApiChatInfo>().PostCustomProfile(dictProfile);
            if (!status)
            {
                throw new Exception("Change profile failed");
            }
            
            Signal.Send(StreamId.UI.BackToSwipe);
            this.PostEvent(TypeGameEvent.GameStart);
            this.HideProcessing();
        }
        catch (Exception e)
        {
            Signal.Send(StreamId.UI.BackToSwipe);
            this.PostEvent(TypeGameEvent.GameStart);
            e.ShowError();
        }


        // var storageUserInfo = FactoryStorage.Get<StorageUserInfo>();
        // var userInfo = storageUserInfo.Get();
        // userInfo.isCustomizedProfile = true;
        // storageUserInfo.Save();
    }

    private void UpdateProgress()
    {
        sliderProgress.fillAmount = (float) _currentProgress / _maxProgress;
    }  
}
