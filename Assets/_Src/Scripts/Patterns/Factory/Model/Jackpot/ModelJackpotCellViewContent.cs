namespace Game.Model
{
    public class ModelJackpotCellViewContent : AModelJackpotCellView
    {
        public ModelApiEventJackpotData History;
        
        public ModelJackpotCellViewContent()
        {
            Type = TypeJackpotCellView.Content;
        }
    }
}