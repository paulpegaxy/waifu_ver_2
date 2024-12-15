using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Model;
using Game.Runtime;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Game.GameTools
{
    [Serializable]
    [Factory(CollectionType.Rarities, true)]
    public class EditorModuleRarity : EditorModule<ModelRarity>
    {
        public EditorModuleRarity()
        {
            _type = CollectionType.Rarities;
        }

        protected override void AddMenu(ModelRarity item)
        {
            var name = CollectionType.Rarities.ToString().PascalToSpace();
            var index = ((int)item.Type).ToString().PadLeft(2, '0');
            _tree.Add($"{name}/{index}-{item.Type.ToString().PascalToSpace()}", item);
        }

        protected override void Sort()
        {
            _dataAll = _dataAll.OrderBy(x => (int)x.Type).ToList();
        }

        protected override bool ValidateCreate()
        {
            for (int i = 0; i < _dataAll.Count; i++)
            {
                var ele = _dataAll[i];
                if (ele.Type == _data.Type)
                {
                    UnityEngine.Debug.LogError($"Existed this item {_data.Type}!");
                    return false;
                }
            }

            return true;
        }

        protected override void DeleteItem(ModelRarity item)
        {
            _dataAll.RemoveAll(x => x.Type == item.Type);
        }
    }
}