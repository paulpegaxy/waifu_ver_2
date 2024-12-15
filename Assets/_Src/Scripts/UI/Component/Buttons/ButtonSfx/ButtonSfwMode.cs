using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Runtime;
using Sirenix.Utilities;
using Template.Defines;
using UnityEngine;

public class ButtonSfwMode : MonoBehaviour
{
    [SerializeField] private UIButton btnClick;
    [SerializeField] private GameObject[] arrObjNotify;
    [SerializeField] private GameObject objActive;
    
    private bool _isInitiated;
    
    private void OnEnable()
    {
        btnClick.onClickEvent.AddListener(OnClick);
        if (!_isInitiated)
        {
            _isInitiated = true;
            return;
        }

        if (FactoryApi.Get<ApiGame>().Data.Info == null)
            return;
        
        Refresh();
    }
    
    private void OnDisable()
    {
        btnClick.onClickEvent.RemoveListener(OnClick);
    }

    private void Refresh()
    {
        var storageUserInfo= FactoryStorage.Get<StorageUserInfo>();
        
        var userInfo = storageUserInfo.Get();
        if (SpecialExtensionTutorial.IsPassTutorial(TutorialCategory.Undress))
        {
            for (var i = 0; i < arrObjNotify.Length; i++)
            {
                arrObjNotify[i].SetActive(!userInfo.isDoneNotifySfwMode);
            }
        }
        else
        {
            arrObjNotify.ForEach(x => x.SetActive(false));
        }

        objActive.SetActive(userInfo.isActiveSfwMode);
    }

    private void OnClick()
    {
        var storageUserInfo= FactoryStorage.Get<StorageUserInfo>();
        var userInfo = storageUserInfo.Get();
        userInfo.isActiveSfwMode = !userInfo.isActiveSfwMode;
        if (!userInfo.isDoneNotifySfwMode)
            userInfo.isDoneNotifySfwMode = true;
            
        storageUserInfo.Save();
        Refresh();
        this.PostEvent(TypeGameEvent.ChangeSfwMode);
    }
}
