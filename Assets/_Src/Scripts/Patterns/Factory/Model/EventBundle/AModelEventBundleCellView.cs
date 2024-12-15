

using System;
using Game.Model;
using Game.UI;

namespace Game.UI
{
    [Serializable]
    public abstract class AModelEventBundleCellView : IESModel<TypeEventBundleCellView>
    {
        public string EventId;
        public ModelApiShopData DataBundle;
        public TypeEventBundleCellView Type { get; set; }
        
        public AModelEventBundleCellView(string eventId)
        {
            this.EventId = eventId;
        }
        
        public AModelEventBundleCellView(string eventId,ModelApiShopData dataBundle)
        {
            this.EventId = eventId;
            this.DataBundle = dataBundle;
        }
    }

    public enum TypeEventBundleCellView
    {
        Header,
        HeaderTitle,
        PackBg,
        PackOfferTimeLapse,
        PackTap
    }
}