using System;
using Game.Model;
using Game.UI;
using TMPro;
using UnityEngine;

namespace _Src.Scripts.UI.Windows.Implements.Gallery
{
    public class GalleryCellViewHeaderLocation : ESCellView<AModelGalleryCellView>
    {
        [SerializeField] private GalleryItemFilter itemFilterType;


        private TypeFilterGallery _filterType;

        public override void SetData(AModelGalleryCellView data)
        {
            if (data is ModelGalleryCellViewHeaderLocation modelData)
            {
                _filterType = modelData.FilterType;
                itemFilterType.SetData(modelData.FilterType);
            }
        }
    }
}