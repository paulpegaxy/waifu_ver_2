namespace Game.Model
{
    public class ModelTransactionCellViewInfo : AModelTransactionCellView
    {
        public ModelApiUserTonTransactionData Transaction;
        
        public ModelTransactionCellViewInfo()
        {
            Type = TypeTransactionCellView.Info;
        }
    }
}