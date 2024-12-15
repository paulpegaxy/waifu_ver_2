
using Game.UI;
using Template.Defines;

namespace Game.Model
{
    public abstract class 
        ModelBuddyCellView : IESModel<BuddyCellViewType>
    {
        public BuddyCellViewType Type { get; set; }
    }
}
