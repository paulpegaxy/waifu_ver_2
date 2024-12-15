using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class ScrollRectNested : ScrollRect
    {
        private bool _routeToParent = false;

        private void DoForParents<T>(Action<T> action) where T : IEventSystemHandler
        {
            var parent = transform.parent;
            while (parent != null)
            {
                foreach (var component in parent.GetComponents<Component>())
                {
                    if (component is T)
                    {
                        action((T)(IEventSystemHandler)component);
                    }
                }
                parent = parent.parent;
            }
        }

        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            DoForParents<IInitializePotentialDragHandler>((parent) => { parent.OnInitializePotentialDrag(eventData); });
            base.OnInitializePotentialDrag(eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (_routeToParent)
            {
                DoForParents<IDragHandler>((parent) => { parent.OnDrag(eventData); });
            }
            else
            {
                base.OnDrag(eventData);
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (!horizontal && Math.Abs(eventData.delta.x) > Math.Abs(eventData.delta.y))
            {
                _routeToParent = true;
            }
            else if (!vertical && Math.Abs(eventData.delta.x) < Math.Abs(eventData.delta.y))
            {
                _routeToParent = true;
            }
            else
            {
                _routeToParent = false;
            }

            if (_routeToParent)
            {
                DoForParents<IBeginDragHandler>((parent) => { parent.OnBeginDrag(eventData); });
            }
            else
            {
                base.OnBeginDrag(eventData);
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (_routeToParent)
            {
                DoForParents<IEndDragHandler>((parent) => { parent.OnEndDrag(eventData); });
            }
            else
            {
                base.OnEndDrag(eventData);
            }

            _routeToParent = false;
        }
    }
}
