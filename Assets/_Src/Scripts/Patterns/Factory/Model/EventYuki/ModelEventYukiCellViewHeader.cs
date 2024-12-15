using System;
using Game.Model;
using UnityEngine.Serialization;

namespace Game.UI
{
    [Serializable]
    public class ModelEventYukiCellViewHeader : AModelEventYukiCellView
    {
        [FormerlySerializedAs("EventData")] public ModelApiEventConfig eventConfig;
        
        public ModelEventYukiCellViewHeader()
        {
            Type = TypeEventYukiCellView.Header;
        }
    }
}