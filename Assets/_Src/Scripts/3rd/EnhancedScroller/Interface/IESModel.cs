using System;

namespace Game.UI
{
	public interface IESModel<TType> where TType : Enum
	{
		TType Type { get; set; }
	}
}