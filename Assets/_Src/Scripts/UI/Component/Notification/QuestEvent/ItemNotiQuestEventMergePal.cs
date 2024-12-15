using System.Collections.Generic;
using Game.Model;
using Game.Runtime;
using Game.UI.QuestEvent;

namespace Game.UI
{
    public class ItemNotiQuestEventMergePal : AItemNotiQuestEvent
    {
        protected override List<ModelApiQuestData> OnSetQuestEventData()
        {
            var eventData = FactoryApi.Get<ApiEvent>().Data.GetEventByType(MainWindowAction.PartnerMergePal);
            if (eventData == null)
                return new List<ModelApiQuestData>();
            
            //Will continue process
            return FactoryApi.Get<ApiQuest>().Data.GetListQuestEvent(eventData.id);
        }
    }
}