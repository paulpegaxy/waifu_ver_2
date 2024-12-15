using System.Collections.Generic;
using Game.UI;

namespace Game.Model
{
    public class ModelGalleryCellViewContentWaifu : AModelGalleryCellView
    {
        public List<DataItemGalleryWaifu> RowItemData;
        
        public ModelGalleryCellViewContentWaifu()
        {
            Type = TypeGalleryCellView.ContentWaifu;
        }
    }
}