using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using EnhancedUI.EnhancedScroller;
using Game.Model;
using Game.Runtime;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class PopupOffer : MonoBehaviour
    {
        [SerializeField] private ShopScrollerOffer scrollerOffer;
        [SerializeField] private ESFlickSnap flickSnap;
        [SerializeField] private UIButton buttonNext;
        [SerializeField] private UIButton buttonPrev;
        [SerializeField] private GameObject pages;
        [SerializeField] private GameObject pageCurrent;

        private int _currentIndex;
        private int _count;

        private void Start()
        {
            var dataList = new List<ModelShopOfferCellView>();
            var offers = ControllerShopOffer.Offers;

            foreach (var offer in offers)
            {
                if (!offer.IsExpired())
                {
                    dataList.Add(new ModelShopOfferCellView
                    {
                        Type = offer.Type,
                        ShopItem = offer.Data
                    });
                }
            }

            _currentIndex = 0;
            _count = dataList.Count;

            flickSnap.SetMaxItems(_count);
            scrollerOffer.SetData(dataList);

            for (var i = 0; i < pages.transform.childCount; i++)
            {
                var page = pages.transform.GetChild(i);
                page.gameObject.SetActive(i < _count);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(pages.GetComponent<RectTransform>());

            Refresh();

            flickSnap.OnJumpComplete += OnJumpComplete;
            buttonNext.onClickEvent.AddListener(OnNext);
            buttonPrev.onClickEvent.AddListener(OnPrev);
        }

        private void OnDestroy()
        {
            flickSnap.OnJumpComplete -= OnJumpComplete;
            buttonNext.onClickEvent.RemoveListener(OnNext);
            buttonPrev.onClickEvent.RemoveListener(OnPrev);
        }

        private void OnNext()
        {
            if (_currentIndex == _count - 1) return;
            _currentIndex++;

            scrollerOffer.scroller.JumpToDataIndex(
                _currentIndex,
                tweenType: EnhancedScroller.TweenType.easeOutSine,
                tweenTime: 0.25f,
                jumpComplete: () => OnJumpComplete(_currentIndex)
            );
        }

        private void OnPrev()
        {
            if (_currentIndex == 0) return;
            _currentIndex--;

            scrollerOffer.scroller.JumpToDataIndex(
                _currentIndex,
                tweenType: EnhancedScroller.TweenType.easeOutSine,
                tweenTime: 0.25f,
                jumpComplete: () => OnJumpComplete(_currentIndex)
            );
        }

        private void OnJumpComplete(int index)
        {
            _currentIndex = index;
            Refresh();
        }

        private void Refresh()
        {
            pageCurrent.transform.position = pages.transform.GetChild(_currentIndex).position;
            buttonNext.gameObject.SetActive(_currentIndex < _count - 1);
            buttonPrev.gameObject.SetActive(_currentIndex > 0);
        }
    }

}