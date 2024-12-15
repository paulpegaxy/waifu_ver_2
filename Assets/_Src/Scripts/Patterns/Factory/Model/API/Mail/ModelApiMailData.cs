using System;
using System.Collections.Generic;

namespace Game.Model
{
	[Serializable]
	public class ModelApiMailData
	{
		public int id;
		public string title;
		public string description;
		public string extra_data;
		public bool is_read;
		public bool is_claimed;
		public DateTime createdAt;
		public DateTime updatedAt;
		public List<ModelApiItemData> rewards;
	}
}