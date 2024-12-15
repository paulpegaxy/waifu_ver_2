using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Game.Model
{
	[Serializable]
	public class ModelApiFriendInvited
	{
		public int offset;
		public int limit;
		public int total;
		public List<ModelApiFriendInvitedData> data;
	}

	[Serializable]
	public class ModelApiFriendInvitedData
	{
		public ModelApiUserData user;
		public int rank;
		public int total_berry;
		public int total_invited;
		
		public int cooldown_slap;
	}
}