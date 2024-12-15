using Game.Model;

namespace Game.UI
{
    public class ModelEventBundleCellViewPackTap : AModelEventBundleCellView
    {
        public ModelEventBundleCellViewPackTap(string eventId,ModelApiShopData dataBundle) : base(eventId,dataBundle)
        {
            Type = TypeEventBundleCellView.PackTap;
        }
    }
}