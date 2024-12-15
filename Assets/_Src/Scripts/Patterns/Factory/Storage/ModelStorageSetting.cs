using System;
using System.Collections.Generic;
using Game.Defines;
using Template.Defines;
using UnityEngine.Serialization;

namespace Game.Model
{
	[Serializable]
	public class ModelStorageSetting
	{
		public bool isSound = true;
		public bool isMusic = true;
		public bool isUseBotTap = false;
		public bool vibration;
		public bool isAuto = false;
		public bool dontShowJackpot;
		public TypeQuality quality;
		public TypeLanguage language = TypeLanguage.English;

		public Dictionary<int, bool> dictLvAlreadyReadMessage = new();
		public int currentLevelGirl;
	}
}