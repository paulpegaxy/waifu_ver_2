using System;
using System.Linq;
using Game.Core;
using Game.Runtime;
using Game.Model;

namespace Game.GameTools
{
    [Serializable]
    [Factory(CollectionType.Tutorial, true)]
    public class EditorModuleTutorial : EditorModule<ModelTutorial>
    {
        public EditorModuleTutorial()
        {
            _type = CollectionType.Tutorial;
        }

        protected override void AddMenu(ModelTutorial item)
        {
            var name = CollectionType.Tutorial.ToString().PascalToSpace();
            var index = item.Category.ToString();

            foreach (var step in item.Steps)
            {
                _tree.Add($"{name}/{index}/{step.State}", step);
            }

            _tree.Add($"{name}/{index}", item);
        }

        protected override bool ValidateCreate()
        {
            for (int i = 0; i < _dataAll.Count; i++)
            {
                var ele = _dataAll[i];
                if (ele.Category == _data.Category)
                {
                    UnityEngine.Debug.LogError($"Existed this item {_data.Category}!");
                    return false;
                }
            }

            return true;
        }

        protected override void DeleteItem(ModelTutorial item)
        {
            _dataAll.RemoveAll(x => x.Category == item.Category);
        }

        protected override void Sort()
        {
            _dataAll = _dataAll.OrderBy(x => x.Category).ToList();
            foreach (var item in _dataAll)
            {
                item.Steps = item.Steps.OrderBy(x => x.State).ToList();
            }
        }
    }
}