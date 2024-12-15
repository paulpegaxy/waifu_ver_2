using Game.Defines;
using UnityEngine;

namespace Game.Model
{
	public class ModelDatingCellViewContentOtherMessage : ModelDatingCellView
	{
		public Sprite SprAvatar;
		public ModelApiEntityConfig Config;
		
		public ModelDatingCellViewContentOtherMessage()
		{
			Type = DatingCellViewType.ContentOtherMesssage;
		}
	}
}