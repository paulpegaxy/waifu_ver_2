using Game.UI;

namespace Game.Model
{
    public class ModelGalleryCellViewHeaderLocation : AModelGalleryCellView
    {
        public TypeFilterGallery FilterType;
    
        public ModelGalleryCellViewHeaderLocation()
        {
            Type = TypeGalleryCellView.HeaderLocation;
        }
    }
}