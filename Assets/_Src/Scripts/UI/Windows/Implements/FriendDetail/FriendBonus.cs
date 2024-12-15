using System.Collections.Generic;
using UnityEngine;
using Game.Model;

namespace Game.UI
{
	public class FriendBonus : MonoBehaviour
	{
		[SerializeField] private Transform posContain;
		[SerializeField] private Color[] arrColorRow;

		public void SetData(List<ModelFriendConfigFriendBonus> models)
		{
			posContain.FillData<ModelFriendConfigFriendBonus, FriendBonusItem>(models, (data, view, index) =>
			{
				view.SetData(data, arrColorRow[index % 2 == 0 ? 0 : 1]);
			});
		}
	}
}