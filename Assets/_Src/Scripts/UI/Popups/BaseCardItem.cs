using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

namespace Slime.UI
{
    public abstract class BaseCardItem<T> : MonoBehaviour
    {
        protected T Data;
        protected int IndexItem;

        private UIContainer _uiContainer;

        protected UIContainer UIContainer => _uiContainer ??= GetComponent<UIContainer>();

        protected abstract void OnSetData();

        public virtual void SetData(T data, int index = 0)
        {
            Data = data;
            IndexItem = index;
            OnSetData();
        }

        public virtual void ShowCard()
        {
            gameObject.SetActive(true);
            if (UIContainer != null)
                UIContainer.Show();
        }

        public virtual void HideCard()
        {
            if (UIContainer != null)
                UIContainer.Hide();
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}