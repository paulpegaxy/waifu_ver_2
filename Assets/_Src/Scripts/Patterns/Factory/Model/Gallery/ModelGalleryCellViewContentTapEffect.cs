using System.Collections.Generic;
using Game.UI;

namespace Game.Model
{
    public class ModelGalleryCellViewContentTapEffect : AModelGalleryCellView
    {
        public List<DataItemGalleryTapEffect> RowItemData;
        
        public ModelGalleryCellViewContentTapEffect()
        {
            Type = TypeGalleryCellView.ContentTapEffect;
        }
    }
}