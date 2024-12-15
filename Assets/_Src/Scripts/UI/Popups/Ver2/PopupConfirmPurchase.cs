using System;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using TMPro;
using Game.Runtime;
using Game.Model;
using Template.Defines;

namespace Game.UI
{
    public class PopupConfirmPurchase : MonoBehaviour
    {
        [SerializeField] private TMP_Text textDescription;
        [SerializeField] private TMP_Text textValue;
        [SerializeField] private Image imageIcon;
        [SerializeField] private UIButton buttonConfirm;

        private ModelResource _data;
        private Action<ModelResource,UIPopup> _onConfirm;

        private void Start()
        {
            buttonConfirm.onClickEvent.AddListener(OnConfirm);
        }

        private void OnDestroy()
        {
            buttonConfirm.onClickEvent.RemoveListener(OnConfirm);
        }

        private void OnConfirm()
        {
            _onConfirm?.Invoke(_data,GetComponent<UIPopup>());
        }

        public void SetData(string description, ModelResource resource, Action<ModelResource,UIPopup> onConfirm)
        {
            _data = resource;
            textDescription.text = description;
            textValue.text = resource.Amount.ToLetter();
            imageIcon.sprite = ControllerSprite.Instance.GetResourceIcon(resource.Type);
            
            _onConfirm = onConfirm;
        }
    }
}