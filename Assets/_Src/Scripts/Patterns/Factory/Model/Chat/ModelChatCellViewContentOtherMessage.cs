namespace Game.Model
{
    public class ModelChatCellViewContentOtherMessage : AModelChatCellView
    {
        public int GirlID;
        public string Message;
        public bool IsPremium;
        
        public ModelChatCellViewContentOtherMessage()
        {
            Type = TypeChatCellView.ContentOtherMessage;
        }
    }
}