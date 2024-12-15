using System;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using BreakInfinity;
using Doozy.Runtime.Signals;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
    public class ItemCurrency : MonoBehaviour
    {
        public static Action<TypeResource> OnAdd;

        [SerializeField] private TypeResource resourceType;
        [SerializeField] private Image imageIcon;
        [SerializeField] private Image imageIconAdd;
        [SerializeField] private TMP_Text textAmount;
        [SerializeField] private UIButton buttonAdd;
        [SerializeField] private bool isUseAddBtn = true;
        [SerializeField] private bool isBigNumber;


        private void Awake()
        {
            if (!isUseAddBtn)
            {
                imageIconAdd.gameObject.SetActive(false);
                textAmount.rectTransform.offsetMax = new Vector2(0, textAmount.rectTransform.offsetMax.y);
                buttonAdd.interactable = false;
            }

            imageIcon.sprite = ControllerSprite.Instance.GetResourceIcon(resourceType);
        }



        private void OnEnable()
        {
            SetAmount(ControllerResource.Get(resourceType).Amount);
            buttonAdd.onClickEvent.AddListener(OnAddClicked);
            ControllerResource.OnChanged += OnChanged;
            ModelApiGameInfo.OnChanged += OnGameInfoChanged;
            if (FactoryApi.Get<ApiChatInfo>().Data.Info != null)
            {
                OnInit();
            }
        }

        private void OnDisable()
        {
            buttonAdd.onClickEvent.RemoveListener(OnAddClicked);
            ControllerResource.OnChanged -= OnChanged;
            ModelApiGameInfo.OnChanged -= OnGameInfoChanged;
        }

        private void OnInit()
        {
            if (resourceType == TypeResource.Ton)
            {
                SetAmount(FactoryApi.Get<ApiGame>().Data.Info.Ton);
            }
        }

        private void OnGameInfoChanged(ModelApiGameInfo gameInfo)
        {
            if (resourceType == TypeResource.Ton)
            {
                SetAmount(gameInfo.Ton);
            }
        }

        private void OnAddClicked()
        {
            // if (resourceType == TypeResource.Berry)
            // {
            if (SpecialExtensionUI.GetCurrentNode() != UIId.UIViewName.Shop.ToString())
            {
                Signal.Send(StreamId.UI.OpenShop);
            }
            // }

            OnAdd?.Invoke(resourceType);
        }

        private void OnChanged(TypeResource type, BigDouble oldAmount, BigDouble newAmount)
        {
            if (resourceType != type) return;
            SetAmount(newAmount);
        }


        private void SetAmount(BigDouble amount)
        {
            if (isBigNumber)
            {
                textAmount.text = amount.ToLetter();
            }
            else
            {
                textAmount.text = amount.ToString();
            }
        }

        private void SetAmount(float value)
        {
            textAmount.text = value.ToDigit5();
        }
    }
}