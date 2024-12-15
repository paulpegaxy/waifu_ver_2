using System.Collections.Generic;
using Game.Extensions;
using Game.UI;
using Template.Defines;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.UI
{
    public class GalleryWindow : UIWindow
    {
        [SerializeField] private List<AGalleryPanel> arrPanel;

        private TypeFilterGallery _filter = TypeFilterGallery.Waifu;

        protected override void OnEnabled()
        {
            var data = this.GetEventData<TypeGameEvent, bool>(TypeGameEvent.OpenPremiumGallery, true);
            if (data)
            {
                _filter = TypeFilterGallery.WaifuPremium;
            }
            else
            {
                _filter = TypeFilterGallery.Waifu;
            }

            ShowPanel(_filter);
            GalleryItemFilter.OnChanged+= ShowPanel;
            GalleryItemLocation.OnChangeBackground += OnRefresh;
            GalleryItemTapEffect.OnChangeTapEffect += OnRefresh;
        }

        protected override void OnDisabled()
        {
            GalleryItemFilter.OnChanged -= ShowPanel;
            GalleryItemLocation.OnChangeBackground -= OnRefresh;
            GalleryItemTapEffect.OnChangeTapEffect -= OnRefresh;
        }

        private void OnRefresh()
        {
            ShowPanel(_filter);
        }
        
        private void ShowPanel(TypeFilterGallery type)
        {
            _filter = type;
            arrPanel.ForEach(x => x.Show(type));
        }
    }
}
