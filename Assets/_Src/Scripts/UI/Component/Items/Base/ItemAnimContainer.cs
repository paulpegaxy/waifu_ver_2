// Author:    -    ad
// Created: 23/07/2024  : : 12:37 AM
// DateUpdate: 23/07/2024

using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

namespace Game.UI
{
    [RequireComponent(typeof(UIContainer))]
    public class ItemAnimContainer : MonoBehaviour
    {
        private UIContainer _container;

        protected UIContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = GetComponent<UIContainer>();
                }

                return _container;
            }
        }
    }
}