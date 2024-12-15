using Game.Model;
using Game.UI;
using UnityEngine;

namespace _Src.Scripts.UI.Windows.Implements.Gallery
{
    public class GalleryCellViewHeaderWaifuPremium: ESCellView<AModelGalleryCellView>
    {
        [SerializeField] private GalleryItemFilter itemFilterType;


        private TypeFilterGallery _filterType;

        public override void SetData(AModelGalleryCellView data)
        {
            if (data is ModelGalleryCellViewHeaderWaifuPremium modelData)
            {
                _filterType = modelData.FilterType;
                itemFilterType.SetData(modelData.FilterType);
            }
        }
    }
}