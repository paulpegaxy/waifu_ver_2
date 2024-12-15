using System;
using System.Collections.Generic;
using Game.Defines;

namespace Game.Model
{
	public class ModelWaifuProfileCellViewContentPicture : ModelWaifuProfileCellView
	{
		public List<DataItemWaifuProfilePicture> RowData;
		
		public ModelWaifuProfileCellViewContentPicture()
		{
			Type = WaifuProfileCellViewType.ContentPicture;
		}
	}

	[Serializable]
	public class DataItemWaifuProfilePicture
	{
		public ModelApiEntityConfig data;
		public int index;
	}
}