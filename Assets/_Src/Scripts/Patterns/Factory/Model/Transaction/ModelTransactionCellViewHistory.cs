namespace Game.Model
{
    public class ModelTransactionCellViewHistory : AModelTransactionCellView
    {
        public ModelApiUserTonHistoryData History;

        public ModelTransactionCellViewHistory()
        {
            Type = TypeTransactionCellView.History;
        }
    }
}