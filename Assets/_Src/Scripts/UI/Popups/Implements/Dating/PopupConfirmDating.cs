using System;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Containers;
using Game.Extensions;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class PopupConfirmDating : PopupConfirm
    {
        [SerializeField] private Image imgGirl;

        private bool _isRightSide;
        private DataItemMatchDating _data;

        private void Start()
        {
            textCancel.text = Localization.Get(TextId.Common_Cancel);
            textOk.text = "Matching";
        }

        public void SetGirl(bool isRightSide)
        {
            _isRightSide = isRightSide;
            var testData = DBM.Config.matchDatingConfig.GetRandomItem();

            _data = testData;
            
            var girlConfig = DBM.Config.rankingConfig.GetRankDataBasedGirlId(testData.girlId);
            textDescription.text = "Do you want to date with " + girlConfig.girlName + "?";
            textDescription.text += $"\n(Rate: {testData.rateMatching.ToPercentString()})";
        }

        protected override void OnOk()
        {
            if (_data.IsMatchingSuccess())
            {
                ControllerPopup.ShowToastSuccess("Matching success!");
                // this.PostEvent(TypeGameEvent.OpenDating, new EventDataDating()
                // {
                //     isOpenRightSide = _isRightSide,
                //     data = _data
                // });
                // Signal.Send(StreamId.UI.Dating);
            }
            else
            {
                ControllerPopup.ShowToastError("Matching fail!");
            }
            GetComponent<UIPopup>().Hide();
        }
    }
}