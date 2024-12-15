using System;
using Cysharp.Threading.Tasks;
using Game.Runtime;
using Slime.UI;
using UnityEngine;

namespace Game.UI.Ver2.Swipe.Item
{
    public abstract class AItemSwipeCharOverlay :  BaseCardItem<DataItemSwipeChar>
    {
        [SerializeField] private ItemUICharTagInfo itemCharInfo;

        public static Action<bool> OnSeeRateInfo;
        public static Action OnUndoCharacter;
        public static Action OnRaiseDeclineGirl;
        public static Action OnRaiseAcceptGirl;

        public override void SetData(DataItemSwipeChar data, int index = 0)
        {
            base.SetData(data, index);
            itemCharInfo.SetData(data.entityConfig);
        }
        
        protected void OnUndo()
        {
            OnUndoCharacter?.Invoke();
        }
        
        protected async void OnDeclineGirl()
        {
            OnRaiseDeclineGirl?.Invoke();
            // this.ShowProcessing();
            // try
            // {
            //     await FactoryApi.Get<ApiEntity>().PostDeclineCharacter(Data.entityConfig.id);
            //     ControllerPopup.ShowToastSuccess("Decline success");
            //     this.HideProcessing();
            // }
            // catch (Exception e)
            // {
            //     e.ShowError();
            // }
        }

        protected async void OnAcceptGirl()
        {
            OnRaiseAcceptGirl?.Invoke();
            // this.ShowProcessing();
            // try
            // {
            //     var status=await FactoryApi.Get<ApiEntity>().PostAcceptCharacter(Data.entityConfig.ID);
            //     if (status)
            //     {
            //         var popup = this.ShowPopup<PopupSuccessMatch>(UIId.UIPopupName.PopupSuccessMatch);
            //         popup.SetData(Data.entityConfig);
            //     }
            //     else
            //     {
            //         ControllerPopup.ShowInformation("Accept fail!\nThis girl don't like you! Please try again");
            //     }
            //     this.HideProcessing();
            // }
            // catch (Exception e)
            // {
            //     e.ShowError();
            // }
        }
    }
}