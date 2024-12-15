using System.Collections.Generic;
using Game.Defines;
using Game.Model;
using Template.Defines;

namespace Game.Runtime
{
	[Factory(StorageType.Settings, true)]
	public class StorageSettings : Storage<ModelStorageSetting>
	{
		public StorageSettings()
		{
			_key = GetKey(StorageType.Settings);
		}

		protected override void InitModel()
		{
			_model = new ModelStorageSetting()
			{
				isSound = true,
				isMusic = true,
				vibration = true,
				isUseBotTap = false,
				quality = TypeQuality.High,
				dictLvAlreadyReadMessage = new (),
				currentLevelGirl = -1,
				language = TypeLanguage.English
			};
		}
	}
}