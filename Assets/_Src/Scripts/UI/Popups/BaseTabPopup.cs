using System;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;

namespace Slime.UI
{
    public class BaseTabPopup : MonoBehaviour
    {
        [SerializeField] protected UIToggleGroup toggleTabs;
        [SerializeField] private List<BasePanel> panels;
        
        private void Start()
        {
            toggleTabs.OnToggleTriggeredCallback.AddListener(OnTabClick);
            toggleTabs.toggles[0].SetIsOn(true, animateChange: false);
        }

        private void OnDestroy()
        {
            toggleTabs.OnToggleTriggeredCallback.RemoveListener(OnTabClick);
        }

        protected virtual void OnTabClick(UIToggle toggle)
        {
            var index = toggleTabs.lastToggleOnIndex;
            if (index < 0 || index >= panels.Count)
                return;
            
            try
            {
                panels[index].Fetch();
            }
            catch (Exception e)
            {
                e.ShowException();
            }
        }
    }
}