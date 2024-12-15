// Author: ad   -
// Created: 17/10/2024  : : 04:10
// DateUpdate: 17/10/2024

using System;
using System.Collections.Generic;
using System.Linq;
using _Src.Scripts.UI.Popups;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public class ItemManageButtonGirl : MonoBehaviour
    {
        [SerializeField] private List<EleButtonGirlCardOverlay> listCardOverlay;

        private int InitGirlId;
        private bool _isPremium;
        private TypeButtonGirlCard _type;

        private void Awake()
        {
            Clear();
        }

        private void Clear()
        {
            for (var i = 0; i < listCardOverlay.Count; i++)
            {
                listCardOverlay[i].refData.gameObject.SetActive(false);
            }
        }

        public void SetData()
        {
            var storageUserInfo = FactoryStorage.Get<StorageUserInfo>();
            var userInfo = storageUserInfo.Get();
            InitGirlId = userInfo.selectedWaifuId;
            _isPremium = userInfo.isChoosePremiumWaifu;
            
            _type = ProcessType();
            
          
        }

        private void OnEnable()
        {
            if (FactoryApi.Get<ApiGame>().Data.Info == null)
                return;
            
            ModelApiGameInfo.OnChanged += OnGameInfoChanged;
            ModelApiUpgradeInfo.OnChanged += OnUpgradeInfoChanged;
            var data = this.GetEventData<TypeGameEvent, int>(TypeGameEvent.ChangeGirl);
            if (data > 0)
            {
                if (data != InitGirlId)
                {
                    var infoGet = FactoryStorage.Get<StorageUserInfo>().Get();
                    InitGirlId = infoGet.selectedWaifuId;
                    _isPremium = infoGet.isChoosePremiumWaifu;
                    Clear();
                    _type = ProcessType();
                    ReloadCard();
                }
            }
        }

        private void OnDisable()
        {
            ModelApiGameInfo.OnChanged -= OnGameInfoChanged;
            ModelApiUpgradeInfo.OnChanged -= OnUpgradeInfoChanged;
        }
        
        protected virtual void OnGameInfoChanged(ModelApiGameInfo gameInfo)
        {
            // var userInfo = FactoryStorage.Get<StorageUserInfo>().Get();
            // if (!userInfo.isChoosePremiumWaifu)
            // {
            //     if (InitGirlId != gameInfo.CurrentGirlId)
            //     {
                    // InitGirlId = gameInfo.CurrentGirlId;
                    var type = ProcessType();
                    if (type!=_type)
                    {
                        _type = type;
                        Clear();
                    }
                    ReloadCard();
            //     }
            // }
        }

        protected virtual void OnUpgradeInfoChanged(ModelApiUpgradeInfo upgradeInfo)
        {

            var type = ProcessType();
            if (type != _type)
            {
                _type = type;
                Clear();
            }

            ReloadCard();
        }

        private TypeButtonGirlCard ProcessType()
        {
            TypeButtonGirlCard type = TypeButtonGirlCard.None;
            if (_isPremium)
            {
                var apiUpgradeInfo = FactoryApi.Get<ApiUpgrade>().Data;
                var charPremium = apiUpgradeInfo.GetPremiumChar(InitGirlId);
                if (charPremium != null)
                {
                    if (charPremium.level >= GameConsts.MAX_LEVEL_PER_CHAR)
                        type = TypeButtonGirlCard.ChangeGirlPremium;
                    else
                    {
                        var cooldown = charPremium.GetCooldownUndress();
                        if (cooldown > 0)
                            type = TypeButtonGirlCard.UndressPremiumCooldown;
                        else
                            type = TypeButtonGirlCard.UndressPremium;
                    }
                }
            }
            else
            {
                if (ServiceLocator.GetService<IServiceValidate>().CanNextGirl())
                {
                    type = TypeButtonGirlCard.NextGirl;
                }
                else
                {
                    type = TypeButtonGirlCard.Undress;
                }
            }

            return type;
        }

        private void ReloadCard()
        {
            var itemFind = listCardOverlay.Find(x => x.type == _type);
            if (itemFind != null)
            {
                itemFind.refData.ShowCard();
                itemFind.refData.SetData(new DataCardButtonGirlItem()
                {
                    type = _type,
                    girlId = InitGirlId
                });
            }
            else
            {
                UnityEngine.Debug.LogError("not found type at manager");
                ControllerPopup.ShowToastError("Not found type at manager button girl");
            }
        }
    }

    [Serializable]
    public class DataCardButtonGirlItem
    {
        public TypeButtonGirlCard type;
        public int girlId;
    }
        

    public enum TypeButtonGirlCard
    {
        None,
        Undress,
        NextGirl,
        UndressPremium,
        ChangeGirlPremium,
        UndressPremiumCooldown,
    }
    
    [Serializable]
    public class EleButtonGirlCardOverlay : BaseEleUIType<TypeButtonGirlCard,AItemButtonGirlCardOverlay>
    {
        
    }
}