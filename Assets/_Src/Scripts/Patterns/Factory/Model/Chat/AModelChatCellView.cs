using Game.UI;
using Template.Defines;

namespace Game.Model
{
    public abstract class AModelChatCellView : IESModel<TypeChatCellView>
    {
        public TypeChatCellView Type { get; set; }
    }

    public enum TypeChatCellView
    {
        ContentOverview,
        ContentOtherMessage,
        COntentMyMessage
    }
}