using System.Collections.Generic;
using Game.Model;
using Game.Runtime;
using Game.UI.QuestEvent;

namespace Game.UI
{
    public class ItemNotiQuestEventYuki : AItemNotiQuestEvent
    {
        protected override List<ModelApiQuestData> OnSetQuestEventData()
        {
            var eventData = FactoryApi.Get<ApiEvent>().Data.EventMeetYuki;
            if (eventData == null)
                return new List<ModelApiQuestData>();
            
            return FactoryApi.Get<ApiQuest>().Data.GetListQuestEvent(eventData.id);
        }
    }
}