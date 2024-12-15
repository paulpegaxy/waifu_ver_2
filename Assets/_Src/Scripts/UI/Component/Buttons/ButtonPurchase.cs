using System;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using Template.Defines;

namespace Game.UI
{
    public class ButtonPurchase : MonoBehaviour
    {
        [SerializeField] private TMP_Text textValueFree;
        [SerializeField] private TMP_Text textValueTon;
        [SerializeField] private TMP_Text textValuePal;
        [SerializeField] private TMP_Text textPricePal;
        [SerializeField] private UIButton buttonFree;
        [SerializeField] private UIButton buttonTon;
        [SerializeField] private UIButton buttonPal;

        public Action<TypePurchase> OnPurchase;

        private void OnEnable()
        {
            buttonFree.onClickEvent.AddListener(OnFree);
            buttonTon.onClickEvent.AddListener(OnTon);
            buttonPal.onClickEvent.AddListener(OnPal);
        }

        private void OnDisable()
        {
            buttonFree.onClickEvent.RemoveListener(OnFree);
            buttonTon.onClickEvent.RemoveListener(OnTon);
            buttonPal.onClickEvent.RemoveListener(OnPal);
        }

        private void OnFree()
        {
            OnPurchase?.Invoke(TypePurchase.Free);
        }

        private void OnTon()
        {
            OnPurchase?.Invoke(TypePurchase.Ton);
        }

        private void OnPal()
        {
            OnPurchase?.Invoke(TypePurchase.Pal);
        }

        public void SetData(string valueFree, string valueTon, string valuePal, string pricePal)
        {
            textValueFree.text = valueFree;
            textValueTon.text = valueTon;
            textValuePal.text = valuePal;
            textPricePal.text = pricePal;
        }
    }
}