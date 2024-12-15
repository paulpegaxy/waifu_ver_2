using Game.UI;

namespace Game.Model
{
    public class AModelJackpotCellView : IESModel<TypeJackpotCellView>
    {
        public TypeJackpotCellView Type { get; set; }
    }
    
    public enum TypeJackpotCellView
    {
        Header,
        Content,
    }
}