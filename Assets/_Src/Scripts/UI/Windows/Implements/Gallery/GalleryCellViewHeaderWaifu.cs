using System;
using Game.Model;
using Game.UI;
using UnityEngine;

namespace _Src.Scripts.UI.Windows.Implements.Gallery
{
    public class GalleryCellViewHeaderWaifu : ESCellView<AModelGalleryCellView>
    {
        [SerializeField] private GalleryItemFilter itemFilter;

        private TypeFilterGallery _filterType;

        public override void SetData(AModelGalleryCellView data)
        {
            if (data is ModelGalleryCellViewHeader modelData)
            {
                _filterType = modelData.FilterType;
                itemFilter.SetData(modelData.FilterType);
            }
        }
    }
}