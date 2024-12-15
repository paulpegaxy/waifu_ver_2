using Game.UI;

namespace Game.Model
{
    public class ModelGalleryCellViewHeader : AModelGalleryCellView
    {
        public TypeFilterGallery FilterType;
        
        public ModelGalleryCellViewHeader()
        {
            Type = TypeGalleryCellView.Header;
        }
    }
}