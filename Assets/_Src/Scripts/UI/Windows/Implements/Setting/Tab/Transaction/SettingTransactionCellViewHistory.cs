using Game.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SettingTransactionCellViewHistory : ESCellView<AModelTransactionCellView>
    {
        [SerializeField] private TMP_Text textSource;
        [SerializeField] private TMP_Text textDate;
        [SerializeField] private TMP_Text textTon;
        [SerializeField] private TMP_Text textStatus;
        [SerializeField] private Image imgStatus;
        [SerializeField] private Sprite[] arrSprStatus;

        public override void SetData(AModelTransactionCellView model)
        {
            if (model is ModelTransactionCellViewHistory data)
            {
                var history = data.History;

                textSource.text = history.source;
                textDate.text = history.created_at.ToShortDateString();

                var isSuccess = history.status.ToLower() == "success";
                textStatus.color = isSuccess ? GameUtils.GetColor("#86F531") : Color.white;
                textStatus.text = history.status;

                var isIn = history.type.ToLower() == "in";
                textTon.text = (isIn ? "+" : "-") + history.amount;
                textTon.color = isIn ? Color.white : GameUtils.GetColor("#FF9393");
                
                imgStatus.sprite = isSuccess ? arrSprStatus[0] : arrSprStatus[1];
            }
        }
    }
}