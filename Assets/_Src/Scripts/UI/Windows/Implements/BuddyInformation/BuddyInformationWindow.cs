using System.Collections.Generic;
using UnityEngine;
using Game.Model;

namespace Game.UI
{
	public class BuddyInformationWindow : UIWindow
	{
		[SerializeField] private BuddyScroller scrollerInformation;

		protected override void OnEnabled()
		{
			var data = new List<ModelBuddyCellView>
			{
				new ModelBuddyCellViewContentInformation(),
				new ModelBuddyCellViewContentLayer(),
			};

			scrollerInformation.SetData(data);
		}
	}
}