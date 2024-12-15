using System;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using UnityEngine.UI;

namespace Game.UI
{
    public abstract class ItemSelect<T> : MonoBehaviour where T : Enum
    {
        [SerializeField] private TMP_Text textSelect;
        [SerializeField] private UIToggle toggleSelect;
        [SerializeField] private UIToggleGroup toggleItems;

        public Action<T> OnSelected;

        private void OnEnable()
        {
            toggleSelect.onClickEvent.AddListener(OnSelectClick);
            toggleItems.OnToggleTriggeredCallback.AddListener(OnItemSelected);
        }

        private void OnDisable()
        {
            toggleSelect.onClickEvent.RemoveListener(OnSelectClick);
            toggleItems.OnToggleTriggeredCallback.RemoveListener(OnItemSelected);

            toggleSelect.SetIsOn(false);
        }

        private void OnSelectClick()
        {
            var mask = toggleItems.transform.parent;
            if (!mask.gameObject.TryGetComponent<Canvas>(out var canvas))
            {
                canvas = mask.gameObject.AddComponent<Canvas>();
                mask.gameObject.AddComponent<GraphicRaycaster>();
            }

            canvas.overrideSorting = true;
            canvas.sortingOrder = 1;
            canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
        }

        private void OnItemSelected(UIToggle toggle)
        {
            var index = (object)toggleItems.lastToggleOnIndex;
            var type = (T)index;

            toggleSelect.SetIsOn(false);
            textSelect.text = GetText(type);

            OnSelected?.Invoke(type);
        }

        protected virtual string GetText(T value)
        {
            return value.ToString();
        }

        public void Select(T value, bool animateChange = true, bool triggerValueChanged = true)
        {
            var index = Convert.ToInt32(value);
            if (index < 0 || index >= toggleItems.toggles.Count) return;

            textSelect.text = GetText(value);
            toggleItems.toggles[index].SetIsOn(true, animateChange, triggerValueChanged);
        }
    }
}