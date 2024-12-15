using System;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using TMPro;


namespace Game.UI
{
    public class PopupConfirm : MonoBehaviour
    {
        [SerializeField] protected TMP_Text textOk;
        [SerializeField] protected TMP_Text textCancel;
        [SerializeField] protected TMP_Text textDescription;
        [SerializeField] private UIButton buttonOk;
        [SerializeField] private UIButton buttonCancel;

        private Action<UIPopup> _onOk;
        private Action<UIPopup> _onCancel;

        private void Awake()
        {
            buttonOk.onClickEvent.AddListener(OnOk);
            buttonCancel.onClickEvent.AddListener(OnCancel);
        }

        private void OnDestroy()
        {
            buttonOk.onClickEvent.RemoveListener(OnOk);
            buttonCancel.onClickEvent.RemoveListener(OnCancel);
        }

        protected virtual void OnOk()
        {
            _onOk?.Invoke(GetComponent<UIPopup>());
        }
        

        private void OnCancel()
        {
            var popup = GetComponent<UIPopup>();
            if (_onCancel != null)
            {
                _onCancel.Invoke(popup);
            }
            else
            {
                popup.Hide();
            }
        }

        public void SetData(string description, string ok = null, string cancel = null, Action<UIPopup> onOk = null, Action<UIPopup> onCancel = null)
        {
            textDescription.text = description;
            textOk.text = ok ?? Localization.Get(TextId.Common_Ok);
            textCancel.text = cancel ?? Localization.Get(TextId.Common_Cancel);

            _onOk = onOk;
            _onCancel = onCancel;
        }
    }
}