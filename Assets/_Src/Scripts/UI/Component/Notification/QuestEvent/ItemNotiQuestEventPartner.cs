using System.Collections.Generic;
using Game.Model;
using Game.Runtime;
using Game.UI.QuestEvent;
using UnityEngine;

namespace Game.UI
{
    public class ItemNotiQuestEventPartner : AItemNotiQuestEvent
    {
        [SerializeField] private MainWindowAction typeParter;
        
        protected override List<ModelApiQuestData> OnSetQuestEventData()
        {
            var eventData = FactoryApi.Get<ApiEvent>().Data.GetEventByType(typeParter);
            if (eventData == null)
                return new List<ModelApiQuestData>();

            List<ModelApiQuestData> dataList = new List<ModelApiQuestData>();
            // dataList = FactoryApi.Get<ApiQuest>().Data.GetListQuestEvent(eventData.id);

            dataList = SpecialExtensionGame.GetQuestPartnerEventList(FactoryApi.Get<ApiQuest>().Data.Quest, eventData);
            
            // bool isHavePrivate = FactoryApi.Get<ApiUser>().Data.User.IsHavePrivatePartner(eventData.GetPartnerTagPrivate);
            // if (isHavePrivate)
            // {
            //     var listPrivate = FactoryApi.Get<ApiQuest>().Data.GetListQuestEvent(eventData.PrivatePartnerId);
            //     dataList.AddRange(listPrivate);
            // }

            return dataList;
        }
    }
}