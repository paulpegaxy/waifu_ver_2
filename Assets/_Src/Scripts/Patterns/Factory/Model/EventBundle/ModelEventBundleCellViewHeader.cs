namespace Game.UI
{
    public class ModelEventBundleCellViewHeader : AModelEventBundleCellView
    {
        public ModelEventBundleCellViewHeader(string eventId) : base(eventId)
        {
            Type = TypeEventBundleCellView.Header;
        }
    }
}