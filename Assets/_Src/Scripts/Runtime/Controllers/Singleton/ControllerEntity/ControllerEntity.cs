using System;
using Cysharp.Threading.Tasks;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Game.UI;
using Template.Defines;
using UnityEngine;
using UnityEngine.UI;

public class ControllerEntity : GameplayListener
{
    [SerializeField] private RawImage imgBg;
    [SerializeField] private Transform posContainVfxBg;
    [SerializeField] private Transform posSpawnGirl;
    [SerializeField] private Transform posContainSfwObject;
    [SerializeField] private ControllerUndressAnim animUndress;
    [SerializeField] private GameObject holderGroupBtn;
    [SerializeField] private float durationUndress = 1f;
    [SerializeField] private ItemManageButtonGirl itemMangerButtonGirl;


    private int _entityId;
    private string _prevKeyBg;
    private GirlEntity _currentEntity;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.RegisterEvent(TypeGameEvent.UndressGirl, OnUndressGirl);
        this.RegisterEvent(TypeGameEvent.UndressTransitionFade, ProcessUndressGirl);
        this.RegisterEvent(TypeGameEvent.ChangeSfwMode, OnChangeSfwMode);
        
        ModelApiUpgradeInfo.OnChanged += OnChangeData;
        
        var eventChangeGirl = this.GetEventData<TypeGameEvent, int>(TypeGameEvent.ChangeGirl);
        if (eventChangeGirl > 0)
        {
            if (_entityId != eventChangeGirl)
                ReloadNewChar(eventChangeGirl);
        }

        var eventChangeBg = this.GetEventData<TypeGameEvent, bool>(TypeGameEvent.ChangeBackground,true);
        if (eventChangeBg)
        {
            Debug.LogError("ONCHANGE BG");
            OnChangeBackground();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.RemoveEvent(TypeGameEvent.UndressGirl, OnUndressGirl);
        this.RemoveEvent(TypeGameEvent.UndressTransitionFade, ProcessUndressGirl);
        this.RemoveEvent(TypeGameEvent.ChangeSfwMode, OnChangeSfwMode);
    }


    protected override void OnInitialize()
    {
        var data = FactoryApi.Get<ApiUpgrade>().Data;
        if (data.current.background.Count > 0)
        {
            SpawnBackground();
        }

        if (_currentEntity == null)
        {
            // FirstLoadGame(FactoryApi.Get<ApiGame>().Data.Info);
        }
    }

    private void OnChangeData(ModelApiUpgradeInfo data)
    {
        if (data.current.background.Count > 0)
        {
            SpawnBackground();
        }
    }

    protected override void OnGameInfoChanged(ModelApiGameInfo gameInfo)
    {
        var userInfo = FactoryStorage.Get<StorageUserInfo>().Get();
        if (!userInfo.isChoosePremiumWaifu)
        {
            if (_entityId != gameInfo.CurrentGirlId)
            {
                // GameUtils.Log("red","Current Girl " + gameInfo.CurrentGirlId + " , entityId" + _entityId);
                ReloadNewChar(gameInfo.CurrentGirlId);
            }
            else
            {
                var modValue = (gameInfo.current_level_girl + 1) % GameConsts.MAX_LEVEL_PER_CHAR;
                if (modValue==0 || modValue == GameConsts.MAX_LEVEL_PER_CHAR - 1)
                {
                    SpawnBackground();
                }
            }
        }
    }

    private void ReloadNewChar(int girlId)
    {
        this.ShowProcessing();
        _currentEntity.DestroyObject();
        Destroy(_currentEntity.gameObject);
        _currentEntity = null;
        _entityId = girlId;
        SpawnBackground();
        SpawnGirl(this.HideProcessing);
    }

    private void OnChangeBackground()
    {
        var userInfo = FactoryStorage.Get<StorageUserInfo>().Get();
        // SpawnBackground();
    }

    private void FirstLoadGame(ModelApiGameInfo gameInfo)
    {

#if !UNITY_EDITOR && !PRODUCTION_BUILD
        this.ShowPopup(UIId.UIPopupName.PopupInputKey);
#endif
        this.ShowProcessing();
        TelegramWebApp.SetLoadingText("Loading girl...");
        TelegramWebApp.SetLoadingProgress(0.85f);
        
        var storageUserInfo = FactoryStorage.Get<StorageUserInfo>();
        var userInfo = storageUserInfo.Get();
        if (userInfo.selectedWaifuId == 0)
        {
            userInfo.selectedWaifuId = FactoryApi.Get<ApiGame>().Data.Info.CurrentGirlId;
            userInfo.isChoosePremiumWaifu = false;
            storageUserInfo.Save();
        }
        
        var apiUpradeInfo = FactoryApi.Get<ApiUpgrade>().Data.premium_waifu;
        if (apiUpradeInfo == null || apiUpradeInfo.Count <= 0)
        {
            if (userInfo.isChoosePremiumWaifu || userInfo.selectedWaifuId == 30001)
            {
                userInfo.isChoosePremiumWaifu = false;
                userInfo.selectedWaifuId = FactoryApi.Get<ApiGame>().Data.Info.CurrentGirlId;
                storageUserInfo.Save();
            }
        }
        
        _entityId = userInfo.selectedWaifuId;
        HideGirl(gameInfo);
        
        TelegramWebApp.SetLoadingText("Loading background...");
        TelegramWebApp.SetLoadingProgress(0.9f);
        SpawnBackground();

        TelegramWebApp.SetLoadingText("Loading girl skin...");
        TelegramWebApp.SetLoadingProgress(0.99f);

        SpawnGirl(() =>
        {
            this.HideProcessing();
            TelegramWebApp.SetLoadingText("Done load girl");
            this.PostEvent(TypeGameEvent.GameStart);
            itemMangerButtonGirl.SetData();
            this.StartDelayMethod(0.25f, () =>
            {
                TelegramWebApp.SetLoadingProgress(1);
            });
            ProcessSfwMode();
        });
    }

    private void HideGirl(ModelApiGameInfo gameInfo)
    {
        if (gameInfo.IsNeedProtectGirl())
        {
            if (gameInfo.CurrentGirlId != 20009)
                this.ShowPopup(UIId.UIPopupName.PopupProtectGirl);
        }
    }

    private void SpawnGirl(Action callBack)
    {
        _currentEntity = ControllerSpawner.Instance.SpawnGirl(_entityId, posSpawnGirl) as GirlEntity;
        if (_currentEntity != null)
            _currentEntity.Init(_entityId, callBack);
    }

    private async void SpawnBackground()
    {
        if (_entityId == 0) return;


        var storageUserInfo = FactoryStorage.Get<StorageUserInfo>();
        
        var userInfo = storageUserInfo.Get();
        string key =
            DBM.Config.backgroundConfig.GetBackgroundCharNormal(_entityId,
                FactoryApi.Get<ApiGame>().Data.Info.current_level_girl);
        if (key == _prevKeyBg)
            return;
        _prevKeyBg = key;
        
        if (userInfo.isChooseSpecialBg)
        {
            key = userInfo.selectedBackgroundId;
            var configBg = DBM.Config.backgroundConfig.GetBackgroundByBgId(userInfo.selectedBackgroundId);
            if (configBg.isHaveVfx)
            {
                SpawnBgEffect(key).Forget();
                posContainVfxBg.gameObject.SetActive(true);
            }
            else
            {
                posContainVfxBg.gameObject.SetActive(false);
            }
        }
        else
        {
            posContainVfxBg.gameObject.SetActive(false);
        }

        var bg = await ControllerSpawner.Instance.SpawnBackground(key);
        imgBg.texture = bg;
    }

    private async UniTask SpawnBgEffect(string key)
    {
        if (posContainVfxBg.transform.childCount > 0)
        {
            for (int i = posContainVfxBg.transform.childCount - 1; i >= 0; i--)
                DestroyImmediate(posContainVfxBg.GetChild(i).gameObject);
        }

        var eff =await ControllerSpawner.Instance.SpawnAsync(key + "_vfx", true, true, posContainVfxBg);
        eff.transform.localPosition=Vector3.zero;
    }


#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            OnUndressGirl(null);
        }
    }
#endif

    private void ProcessSfwMode()
    {
        bool isActiveSfw = FactoryStorage.Get<StorageUserInfo>().Get().isActiveSfwMode;
        posContainSfwObject.gameObject.SetActive(isActiveSfw);
        posSpawnGirl.gameObject.SetActive(!isActiveSfw);
    }

    private void OnChangeSfwMode(object data)
    {
        ProcessSfwMode();
    }

    private void OnUndressGirl(object obj)
    {
        holderGroupBtn.SetActive(false);
        animUndress.gameObject.SetActive(true);
        animUndress.PlayAnim();
        
        if (_currentEntity != null)
        {
            _currentEntity.Undress(SpecialExtensionGame.GetLevelWaifu(_entityId));
        }
    }

    private async void ProcessUndressGirl(object obj)
    {
        await _currentEntity.DoneUndress(durationUndress);
        animUndress.gameObject.SetActive(false);
        holderGroupBtn.SetActive(true);
    }
}
