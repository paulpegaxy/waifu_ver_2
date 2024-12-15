

namespace Game.Runtime
{
	public static class ExtensionNumber
	{
		public static string ToDigit(this float value)
		{
			return value.ToString("0.##");
		}

		public static string ToDigit5(this float value)
		{
			return value.ToString("0.#####");
		}

		public static string ToFormat(this int value)
		{
			return value.ToString("#,##0");
		}

		public static string ToOrdinal(this int value)
		{
			if (value <= 0) return value.ToString();

			return (value % 100) switch
			{
				11 or 12 or 13 => value + "th",
				_ => (value % 10) switch
				{
					1 => value + "st",
					2 => value + "nd",
					3 => value + "rd",
					_ => value + "th",
				},
			};
		}
	}
}