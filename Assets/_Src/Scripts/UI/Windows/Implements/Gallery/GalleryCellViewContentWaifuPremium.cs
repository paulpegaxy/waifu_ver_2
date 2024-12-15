using System.Collections.Generic;
using Game.Model;
using Game.UI;
using UnityEngine;

namespace _Src.Scripts.UI.Windows.Implements.Gallery
{
    public class GalleryCellViewContentWaifuPremium :  ESCellView<AModelGalleryCellView>
    {
        [SerializeField] private List<GalleryItemWaifuPremium> items;
        
        public override void SetData(AModelGalleryCellView data)
        {
            if (data is ModelGalleryCellViewCotentWaifuPremium modelData)
            {
                int count = modelData.RowItemData.Count;
                for (var i = 0; i < count; i++)
                {
                    var ele = modelData.RowItemData[i];
                    var info = ele.index + i < count ? modelData.RowItemData[ele.index + i] : null;
                    if (info != null)
                    {
                        items[i].SetData(info);
                    }
                    items[i].gameObject.SetActive(info != null);
                }

                if (count < items.Count)
                {
                    for (int i = count; i < items.Count; i++)
                    {
                        items[i].gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}