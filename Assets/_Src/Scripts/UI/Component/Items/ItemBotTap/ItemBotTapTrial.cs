using System;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemBotTapTrial : MonoBehaviour
    {
        [SerializeField] private UIButton btnClick;
        [SerializeField] private Sprite[] arrSprStatus;
        [SerializeField] private Image imgStatus;
        [SerializeField] private ItemTimer itemTimer;

        private bool _isRunning;
        
        private void OnEnable()
        {
            btnClick.onClickEvent.AddListener(OnClick);
        }
        
        private void OnDisable()
        {
            btnClick.onClickEvent.RemoveListener(OnClick);
        }
        
        public void SetData(ModelApiUserGameBot botData)
        {
            gameObject.SetActive(true);
            btnClick.enabled = false;
            
            ControllerAutomation.Start(false);
            itemTimer.SetDuration(botData.TimerRemaining, () =>
            {
                FactoryApi.Get<ApiUser>().Get().Forget();
                ControllerAutomation.Stop();
                gameObject.SetActive(false);
            });
        }

        private void OldSetDataToOnOffBot(ModelApiUserGameBot botData)
        {
            gameObject.SetActive(true);
            btnClick.enabled = false;
            var storageSetting = FactoryStorage.Get<StorageSettings>();
            var model = storageSetting.Get();
            _isRunning = model.isUseBotTap;
            
            ControllerAutomation.Start(false);
            itemTimer.SetDuration(botData.TimerRemaining,()=>{
                FactoryApi.Get<ApiUser>().Get().Forget();
                ControllerAutomation.Stop();
                storageSetting.Get().isUseBotTap = false;
                storageSetting.Save();
                itemTimer.gameObject.SetActive(false);
                gameObject.SetActive(false);
            });

            if (_isRunning)
            {
                ControllerAutomation.Start(false);
                imgStatus.sprite = arrSprStatus[1];
                itemTimer.gameObject.SetActive(true);
                itemTimer.SetDuration(botData.TimerRemaining, () =>
                {
                    FactoryApi.Get<ApiUser>().Get().Forget();
                    ControllerAutomation.Stop();
                    storageSetting.Get().isUseBotTap = false;
                    storageSetting.Save();
                    itemTimer.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                });
            }
            else
            {
                ControllerAutomation.Stop();
                imgStatus.sprite = arrSprStatus[0];
                itemTimer.gameObject.SetActive(false);
            }
        }

        
        private async void OnClick()
        {
            _isRunning = !_isRunning;
            var storageSetting = FactoryStorage.Get<StorageSettings>();
            storageSetting.Get().isUseBotTap = _isRunning;
            storageSetting.Save();

            if (!_isRunning)
            {
                ControllerAutomation.Stop();
                itemTimer.gameObject.SetActive(false);
                imgStatus.sprite = arrSprStatus[0];
            }
            else
            {
                var apiUser=FactoryApi.Get<ApiUser>();
                await apiUser.Get();
            }
        }
    }
}