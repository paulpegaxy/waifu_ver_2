using Cysharp.Threading.Tasks;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

namespace Slime.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BasePanel : MonoBehaviour
    {
        public virtual void Fetch()
        {
            Canvas.DOFade(0, 0);
            OnFetchData();
            Canvas.DOFade(1, 0);
        }

        public virtual void Initialize()
        {
            
        }

        protected abstract UniTask OnFetchData();
        
        private CanvasGroup _canvas;
        protected CanvasGroup Canvas => _canvas ??= GetComponent<CanvasGroup>();

        private UIContainer _uiContainer;

        protected UIContainer UIContainer => _uiContainer ??= GetComponent<UIContainer>();
    }
}