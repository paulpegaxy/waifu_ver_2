using System.Collections.Generic;
using Game.UI;

namespace Game.Model
{
    public class ModelGalleryCellViewContentLocation : AModelGalleryCellView
    {
        public List<DataItemGalleryLocation> RowItemData;
        
        public ModelGalleryCellViewContentLocation()
        {
            Type = TypeGalleryCellView.ContentLocation;
        }
    }
}