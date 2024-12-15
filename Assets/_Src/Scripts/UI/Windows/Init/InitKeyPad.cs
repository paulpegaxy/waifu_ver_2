using System;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

namespace Game.UI
{
    public class PopupInputKey : MonoBehaviour
    {
        [SerializeField] private List<UIButton> listBtnNumber;

        public static Action<bool> OnSuccess;

        private List<int> _patterns = new() { 2, 5, 10, 7, 9, 5 };
        private int _currentIndex;

        private void OnEnable()
        {
            for (var i = 0; i < listBtnNumber.Count; i++)
            {
                var button = listBtnNumber[i];
                var index = i + 1;

                button.onClickEvent.AddListener(() => OnClick(index));
            }
        }

        private void OnDisable()
        {
            for (var i = 0; i < listBtnNumber.Count; i++)
            {
                var button = listBtnNumber[i];
                button.onClickEvent.RemoveAllListeners();
            }
        }

        private void OnClick(int index)
        {
            if (_currentIndex >= _patterns.Count) return;

            if (_patterns[_currentIndex] == index)
            {
                _currentIndex++;
                if (_currentIndex >= _patterns.Count)
                {
                    GetComponent<UIPopup>().Hide();
                    OnSuccess?.Invoke(true);
                }
            }
            else
            {
                _currentIndex = 0;
            }
        }
    }
}