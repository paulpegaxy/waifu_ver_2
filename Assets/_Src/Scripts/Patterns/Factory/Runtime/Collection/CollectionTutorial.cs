using System.Collections.Generic;
using Game.Model;
using Template.Defines;

namespace Game.Runtime
{
	[Factory(CollectionType.Tutorial, true)]
	public class CollectionTutorial : Collection<List<ModelTutorial>>
	{
		public CollectionTutorial()
		{
			_key = GetKey(CollectionType.Tutorial);
		}

		public ModelTutorial Get(TutorialCategory category)
		{
			return _model.Find(x => x.Category == category);
		}
	}
}