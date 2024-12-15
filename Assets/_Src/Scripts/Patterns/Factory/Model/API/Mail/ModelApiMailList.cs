using System;
using System.Collections.Generic;

namespace Game.Model
{
	[Serializable]
	public class ModelApiMailList
	{
		public int page;
		public int take;
		public int total_records;
		public List<ModelApiMailData> data;
	}
}