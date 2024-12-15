
using Game.UI;
using Template.Defines;

namespace Game.Model
{
    public abstract class ModelShopCellView : IESModel<ShopCellViewType>
    {
        public ShopCellViewType Type { get; set; }
    }
}