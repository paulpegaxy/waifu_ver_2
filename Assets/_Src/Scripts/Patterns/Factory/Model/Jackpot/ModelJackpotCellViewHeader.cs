namespace Game.Model
{
    public class ModelJackpotCellViewHeader :AModelJackpotCellView
    {
        public ModelApiEventJackpot Jackpot;
        
        public ModelJackpotCellViewHeader()
        {
            Type = TypeJackpotCellView.Header;
        }
    }
}