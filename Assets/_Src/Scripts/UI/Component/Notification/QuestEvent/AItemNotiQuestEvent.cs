using System.Collections.Generic;
using Game.Model;
using Game.Runtime;

namespace Game.UI.QuestEvent
{
    public abstract class AItemNotiQuestEvent : ItemNotificationGeneric<List<ModelApiQuestData>>
    {
        protected override void OnEnabled()
        {
            SetQuestData();
            ModelApiQuest.OnChanged += OnChanged;
        }

        protected override void OnDisabled()
        {	
            ModelApiQuest.OnChanged -= OnChanged;
        }

        private void OnChanged(ModelApiQuest data)
        {
            SetData(data.Quest);
        }
        
        private void SetQuestData()
        {
            var list = OnSetQuestEventData();
            if (list?.Count > 0)
                SetData(list);
        }

        protected abstract List<ModelApiQuestData> OnSetQuestEventData();

        protected override bool IsValid()
        {
            if (_data == null) 
                return false;

            var itemFind = _data.Find(x => x.can_claim);

            // if (itemFind!=null)
            // {
            //     UnityEngine.Debug.Log("quest can claim: " + itemFind.id + ", name: " + itemFind.description);
            // }
			
            return itemFind != null;
        }
    }
}