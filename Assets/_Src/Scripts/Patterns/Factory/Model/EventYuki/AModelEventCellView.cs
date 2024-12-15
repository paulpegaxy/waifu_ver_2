using System;
using Game.UI;

namespace Game.UI
{
    [Serializable]
    public abstract class AModelEventYukiCellView : IESModel<TypeEventYukiCellView>
    {
        public TypeEventYukiCellView Type { get; set; }
    }

    public enum TypeEventYukiCellView
    {
        Header,
        ContentQuest,
        ContentBackground,
        ContentBundleBotTap,
        CotnentBundleTimelapse
    }
}