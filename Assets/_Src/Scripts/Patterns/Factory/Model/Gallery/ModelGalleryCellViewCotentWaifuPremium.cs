using System.Collections.Generic;
using Game.UI;

namespace Game.Model
{
    public class ModelGalleryCellViewCotentWaifuPremium : AModelGalleryCellView
    {
        public List<DataItemGalleryWaifuPremium> RowItemData;
        
        public ModelGalleryCellViewCotentWaifuPremium()
        {
            Type = TypeGalleryCellView.ContentWaifuPremium;
        }
    }
}