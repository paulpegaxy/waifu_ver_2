using System;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SettingTransactionCellViewInfo : ESCellView<AModelTransactionCellView>
    {
        [SerializeField] private TMP_Text textSource;
        [SerializeField] private TMP_Text textDate;
        [SerializeField] private TMP_Text textTon;
        [SerializeField] private TMP_Text textStatus;
        [SerializeField] private UIButton buttonRefresh;
        [SerializeField] private GameObject objectCheck;
        [SerializeField] private Image imgStatus;
        [SerializeField] private Sprite[] arrSprStatus;

        public static Action<ModelApiUserTonTransactionData> OnRefresh;

        private ModelApiUserTonTransactionData _transaction;

        private void OnEnable()
        {
            buttonRefresh.onClickEvent.AddListener(OnRefreshClick);
        }

        private void OnDisable()
        {
            buttonRefresh.onClickEvent.RemoveListener(OnRefreshClick);
        }

        private void OnRefreshClick()
        {
            OnRefresh?.Invoke(_transaction);
        }

        public override void SetData(AModelTransactionCellView model)
        {
            if (model is ModelTransactionCellViewInfo data)
            {
                var transaction = data.Transaction;

                textSource.text = transaction.source;
                textDate.text = transaction.created_at.ToShortDateString();
                textTon.text = transaction.amount.ToString();
                textStatus.text = transaction.status;

                var isSuccess = transaction.status.ToLower() == "success";
                textStatus.color = isSuccess ? GameUtils.GetColor("#86F531") : Color.white;
                
                imgStatus.sprite = isSuccess ? arrSprStatus[0] : arrSprStatus[1];

                objectCheck.SetActive(isSuccess);
                buttonRefresh.gameObject.SetActive(!isSuccess);

                _transaction = transaction;
            }
        }
    }
}