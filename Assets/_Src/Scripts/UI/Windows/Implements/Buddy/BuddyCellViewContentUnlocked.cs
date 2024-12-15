using UnityEngine;
using Game.Model;

namespace Game.UI
{
	public class BuddyCellViewContentUnlocked : ESCellView<ModelBuddyCellView>
	{
		[SerializeField] private BuddyReward buddyReward;

		public override void SetData(ModelBuddyCellView model)
		{
			var data = model as ModelBuddyCellViewContentUnlocked;
			buddyReward.SetData(data);
		}
	}
}
