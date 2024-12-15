using System;
using System.Collections.Generic;
using Game.Model;
using Template.Defines;

namespace Game.Runtime
{
	[Factory(StorageType.Tutorial, true)]
	public class StorageTutorial : Storage<List<ModelStorageTutorial>>
	{
		public StorageTutorial()
		{
			_key = GetKey(StorageType.Tutorial);
		}

		protected override void InitModel()
		{
			_model = new List<ModelStorageTutorial>()
			{
				new() { Category = TutorialCategory.None, State = TutorialState.MainCompleted },
				new() { Category = TutorialCategory.Main, State = TutorialState.MainFirstTimeLogin },
				new() { Category = TutorialCategory.Booster, State = TutorialState.MainBooster },
				new() { Category = TutorialCategory.Upgrade, State = TutorialState.Upgrade },
				new() { Category = TutorialCategory.Undress, State = TutorialState.Undress },
				new() { Category = TutorialCategory.GameFeature, State = TutorialState.FeatureRanking },
				new() { Category = TutorialCategory.NextGirl, State = TutorialState.NextGirl }
			};
		}

		public override void Load()
		{
			base.Load();
			int length = Enum.GetValues(typeof(TutorialCategory)).Length;
			for (int i = 0; i < length; i++)
			{
				var type = (TutorialCategory)i;
				CheckCategory(type);
			}
			Save();
		}

		private void CheckCategory(TutorialCategory type)
		{
			if (_model.Exists(x => x.Category == type)) 
				return;
			switch (type)
			{
				case TutorialCategory.None:
					_model.Add(new ModelStorageTutorial { Category = TutorialCategory.None, State = TutorialState.MainCompleted });
					break;
				case TutorialCategory.Main:
					_model.Add(new ModelStorageTutorial { Category = TutorialCategory.Main, State = TutorialState.MainFirstTimeLogin });
					break;
				case TutorialCategory.Booster:
					_model.Add(new ModelStorageTutorial { Category = TutorialCategory.Booster, State = TutorialState.MainBooster });
					break;
				case TutorialCategory.Upgrade:
					_model.Add(new ModelStorageTutorial { Category = TutorialCategory.Upgrade, State = TutorialState.Upgrade });
					break;
				case TutorialCategory.Undress:
					_model.Add(new ModelStorageTutorial { Category = TutorialCategory.Undress, State = TutorialState.Undress });
					break;
				case TutorialCategory.GameFeature:
					_model.Add(new ModelStorageTutorial { Category = TutorialCategory.GameFeature, State = TutorialState.FeatureRanking });
					break;
				case TutorialCategory.NextGirl:
					_model.Add(new ModelStorageTutorial { Category = TutorialCategory.NextGirl, State = TutorialState.NextGirl });
					break;
			}
		}

		public ModelStorageTutorial Get(TutorialCategory category)
		{
			return _model.Find(x => x.Category == category);
		}
	}

}