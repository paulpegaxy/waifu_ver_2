using System.Collections.Generic;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
	public class ItemNotificationQuestAll : ItemNotificationGeneric<List<ModelApiQuestData>>
	{
		protected override void OnEnabled()
		{
			SetData(FactoryApi.Get<ApiQuest>().Data.Quest);
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

		protected override bool IsValid()
		{
			if (_data == null) 
				return false;

			var itemFind = _data.Find(x => x.can_claim && string.IsNullOrEmpty(x.event_id));

			// if (itemFind!=null)
			// {
			// 	UnityEngine.Debug.Log("quest can claim: " + itemFind.id + ", name: " + itemFind.description);
			// }
			
			return itemFind != null;
		}
	}
}