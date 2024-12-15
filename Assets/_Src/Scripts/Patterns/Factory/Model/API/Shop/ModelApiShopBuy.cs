using System;

namespace Game.Model
{
	[Serializable]
	public class ModelApiShopBuy
	{
		public int user_id;
		public string bundle_id;
		public ModelApiShopData bundle_data;
		public string order_id;
		public string status;
		public decimal price;
		public string currency;
		public string ton_address;
		public string token_address;
		public int token_decimals = 9;
		public DateTime created_at;
		public long ValidUntilUnix => ((DateTimeOffset)created_at.AddMinutes(10)).ToUnixTimeSeconds();
	}
}