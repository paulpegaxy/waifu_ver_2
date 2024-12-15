using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using Game.Ton;
using Template.Defines;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class SettingPanelTransaction : MonoBehaviour
    {
        [SerializeField] private SettingTransactionScroller scroller;
        [SerializeField] private UIToggleGroup toggleGroup;
        [SerializeField] private UIButton buttonWithDraw;
        [SerializeField] private TMP_Text textTon;

        private bool _isInitialized;

        private void OnEnable()
        {
            toggleGroup.OnToggleTriggeredCallback.AddListener(OnToggle);
            buttonWithDraw.onClickEvent.AddListener(OnWithDraw);

            if (!_isInitialized)
            {
                _isInitialized = true;
                return;
            }
            
            SettingTransactionCellViewInfo.OnRefresh += OnRefresh;

            if (toggleGroup.toggles != null && toggleGroup.toggles.Count > 0)
            {
                toggleGroup.toggles[0].SetIsOn(true);
            }

            ModelApiGameInfo.OnChanged += OnGameInfoChanged;
        }
        
        private void OnDisable()
        {
            buttonWithDraw.onClickEvent.RemoveListener(OnWithDraw);
            toggleGroup.OnToggleTriggeredCallback.RemoveListener(OnToggle);
            SettingTransactionCellViewInfo.OnRefresh -= OnRefresh;
            ModelApiGameInfo.OnChanged -= OnGameInfoChanged;
        }

        private void OnToggle(UIToggle toggle)
        {
            Fetch().Forget();
        }
        
        private void OnGameInfoChanged(ModelApiGameInfo info)
        {
            textTon.text = info.Ton.ToDigit5();
        }

        private async void OnWithDraw()
        {
            var apiGame = FactoryApi.Get<ApiGame>();
            var ton = apiGame.Data.Info.Ton;
            if (ton <= 0)
            {
                // ControllerPopup.ShowToast(Localization.Get(TextId.Toast_NothingWithdraw));
                ControllerPopup.ShowToastError("Nothing to widthdraw");
                return;
            }

            ControllerPopup.SetApiLoading(true);
            try
            {
                if (!TONConnect.IsConnected) await TONConnect.ConnectWalletAsync();

                var apiUser = FactoryApi.Get<ApiUser>();
                var withdrawData = await apiUser.SignWithdrawTon(TONConnect.Wallet.account.userFriendlyAddress);

                await WithDraw(withdrawData);
                await Fetch();
            }
            catch (Exception e)
            {
                e.ShowError();
            }
            ControllerPopup.SetApiLoading(false);
        }

        private async void OnRefresh(ModelApiUserTonTransactionData transaction)
        {
            var isWithdraw = transaction.source.ToLower() == "withdraw";
            if (isWithdraw)
            {
                ControllerPopup.SetApiLoading(true);
                try
                {
                    if (!TONConnect.IsConnected) await TONConnect.ConnectWalletAsync();
                    await WithDraw(transaction);
                }
                catch (Exception e)
                {
                    e.ShowError();
                }
                ControllerPopup.SetApiLoading(false);
            }
        }

        private async UniTask WithDraw(ModelApiClaim data)
        {
            var apiUser = FactoryApi.Get<ApiUser>();
            var hash = await TonApi.Claim(data);

            if (hash == null)
            {
                // ControllerPopup.ShowToast(Localization.Get(TextId.Toast_WithdrawFail));
                ControllerPopup.ShowToastError("Withdraw failed");
            }
            else
            {
                await apiUser.CompleteWithdrawTon(data.id, hash);
                await FactoryApi.Get<ApiGame>().GetInfo();
                // ControllerPopup.ShowToast(Localization.Get(TextId.Toast_ClaimSuccess));
                ControllerPopup.ShowToastSuccess("Claim success");
            }
        }

        private async UniTask Fetch()
        {
            // var apiGame = FactoryApi.Get<ApiGame>();
            //
            // if (apiGame.Data.Info == null) return;
            textTon.text = FactoryApi.Get<ApiGame>().Data.Info.Ton.ToDigit5();
            ControllerPopup.SetApiLoading(true);
            try
            {
                var apiUser = FactoryApi.Get<ApiUser>();
                var isTransaction = toggleGroup.lastToggleOnIndex == 0;
                var data = new List<AModelTransactionCellView>();

                if (isTransaction)
                {
                    var transactions = await apiUser.TonTransactions();
                    foreach (var transaction in transactions)
                    {
                        data.Add(new ModelTransactionCellViewInfo()
                        {
                            Transaction = transaction,
                        });
                    }
                }
                else
                {
                    var histories = await apiUser.TonHistories();
                    foreach (var history in histories)
                    {
                        data.Add(new ModelTransactionCellViewHistory()
                        {
                            History = history,
                        });
                    }
                }
                scroller.SetData(data);
            }
            catch (Exception e)
            {
                e.ShowError();
            }
            ControllerPopup.SetApiLoading(false);
        }
    }
}