using Game.Model;

namespace Game.UI
{
    public class ModelEventBundleCellViewPackOfferTimelapse : AModelEventBundleCellView
    {
        public ModelEventBundleCellViewPackOfferTimelapse(string eventId,ModelApiShopData dataBundle) : base(eventId,dataBundle)
        {
            Type = TypeEventBundleCellView.PackOfferTimeLapse;
        }
    }
}