// Author: ad   -
// Created: 01/12/2024  : : 00:12
// DateUpdate: 01/12/2024

using System;
using Game.Runtime;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelApiGameInfoNew : ModelApiNotification<ModelApiGameInfoNew>
    {
        public int chat_point;
        public int swipe_count;
        public int max_swipe_count;
        public int price_send_chat;
        
        public override void Notification()
        {
            max_swipe_count = GameConsts.MAX_SWIPE_COUNT;
            ControllerResource.Set(TypeResource.ChatPoint, chat_point);
            OnChanged?.Invoke(this);
        }
    }
}