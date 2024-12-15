
using Game.Model;
using UnityEngine;

namespace Game.UI
{
    public abstract class AGalleryPanel : BaseWindowPanel<TypeFilterGallery,AModelGalleryCellView>
    {
        [SerializeField] private TypeFilterGallery typePanel;
        [SerializeField] protected GalleryScroller scroller;
        
        protected bool IsWaitingLoadData;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        protected override bool IsThisPanel(TypeFilterGallery type)
        {
            return typePanel == type;
        }

        protected override void SetData()
        {
        }
    }
}