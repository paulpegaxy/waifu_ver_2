using Game.Model;

namespace Game.UI
{
    public class ModelEventBundleCellViewPackBg : AModelEventBundleCellView
    {
        public ModelEventBundleCellViewPackBg(string eventId,ModelApiShopData dataBundle) : base(eventId,dataBundle)
        {
            Type = TypeEventBundleCellView.PackBg;
        }
    }
}