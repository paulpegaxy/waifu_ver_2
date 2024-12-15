using System;
using System.Collections.Generic;

namespace Game.Model
{
	[Serializable]
	public class ModelApiMailClaim
	{
		public List<int> ids;
		public List<ModelApiItemData> rewards;
	}
}