using Game.UI;

namespace Game.Model
{
    public abstract class AModelGalleryCellView : IESModel<TypeGalleryCellView>
    {
        public TypeGalleryCellView Type { get; set; }
    }

    public enum TypeGalleryCellView
    {
        Header,
        ContentWaifu,
        ContentLocation,
        ContentEmpty,
        ContentWaifuPremium,
        HeaderLocation,
        HeaderWaifuPremium,
        ContentTapEffect
    }
}