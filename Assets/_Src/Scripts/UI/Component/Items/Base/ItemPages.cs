using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;

namespace Game.UI
{
    public class ItemPages : MonoBehaviour
    {
        [SerializeField] private UIToggleGroup toggleGroup;
        [SerializeField] private UIButton buttonNext;
        [SerializeField] private UIButton buttonPrev;
        [SerializeField] private List<ItemPage> pages;

        public static Action<int> OnChanged;

        private int _pageFirstIndex = 0;
        private int _pageIndex = 0;
        private int _pageMax = 5;

        private void OnEnable()
        {
            toggleGroup.OnToggleTriggeredCallback.AddListener(OnPageChanged);
            buttonNext.onClickEvent.AddListener(OnNext);
            buttonPrev.onClickEvent.AddListener(OnPrev);

            SetData(10);
        }

        private void OnDisable()
        {
            toggleGroup.OnToggleTriggeredCallback.RemoveListener(OnPageChanged);
            buttonNext.onClickEvent.RemoveListener(OnNext);
            buttonPrev.onClickEvent.RemoveListener(OnPrev);
        }

        private void OnPageChanged(UIToggle toggle)
        {
            _pageIndex = _pageFirstIndex + toggleGroup.lastToggleOnIndex;
            OnChanged?.Invoke(_pageIndex);

            Refresh();
        }

        private void OnNext()
        {
            if (_pageIndex < _pageMax - 1)
            {
                _pageFirstIndex = CalculateFirstIndex(++_pageIndex);
                toggleGroup.toggles[_pageIndex - _pageFirstIndex].SetIsOn(true);
            }
        }

        private void OnPrev()
        {
            if (_pageIndex > 0)
            {
                _pageFirstIndex = CalculateFirstIndex(--_pageIndex);
                toggleGroup.toggles[_pageIndex - _pageFirstIndex].SetIsOn(true);
            }
        }

        private int CalculateFirstIndex(int pageIndex)
        {
            var firstIndex = _pageFirstIndex;
            if (pageIndex - firstIndex >= pages.Count)
            {
                firstIndex = pageIndex - pages.Count + 1;
            }
            else if (pageIndex < firstIndex)
            {
                firstIndex = pageIndex;
            }

            return firstIndex;
        }

        private void Refresh()
        {
            for (var i = 0; i < pages.Count; i++)
            {
                pages[i].SetData(_pageFirstIndex + i + 1);
            }

            buttonNext.gameObject.SetActive(_pageIndex < _pageMax - 1);
            buttonPrev.gameObject.SetActive(_pageIndex > 0);
        }

        public void SetData(int pageMax)
        {
            for (var i = 0; i < pages.Count; i++)
            {
                pages[i].gameObject.SetActive(i < pageMax);
            }

            _pageFirstIndex = 0;
            _pageIndex = 0;
            _pageMax = pageMax;

            Refresh();
        }
    }
}