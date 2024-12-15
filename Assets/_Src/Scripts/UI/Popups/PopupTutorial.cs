using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Model;
using Game.Runtime;

namespace Game.UI
{
    public class PopupTutorial : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Transform container;
        [SerializeField] private Transform overlay;
        [SerializeField] private Transform hand;
        [SerializeField] private TMP_Text textDescription;
        [SerializeField] private TMP_Text txtUserId;
        [SerializeField] private UIButton btnSkip;

        public static Action OnOverlayClick;
        
        private int _countTap;

        public void SetData(ModelTutorialStep stepData, TutorialData tutorialData)
        {
            btnSkip.gameObject.SetActive(false);
            _countTap = 0;
            var y = new List<int> { 775, 525, 275, 25, -225, -425, -675 }[(int)stepData.Alignment];
            container.localPosition = new Vector3(0, y, 0);

            if (stepData.Interactable)
            {
                if (tutorialData != null) ShowHandWithDelay(tutorialData.transform);
            }
            hand.gameObject.SetActive(stepData.Interactable);
            hand.position = Vector3.one * 100;

            container.gameObject.SetActive(stepData.TextId != TextId.None);
            textDescription.text = Localization.Get(stepData.TextId);

            SetOverlayVisible(stepData.HasOverlay);
            SetOverlay3dObject(true);
            var apiUser = FactoryApi.Get<ApiUser>().Data;
            if (apiUser.User != null)
            {
                txtUserId.text = "ID: " + apiUser.User.Id;
            }
            else
            {
                txtUserId.text = "";
            }
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnOverlayClick?.Invoke();
            // _countTap++;
            // if (_countTap >= GameConsts.MAX_LOG_TAP_FOR_FIRST_TIME)
            // {
            //     btnSkip.gameObject.SetActive(true);
            //     _countTap = 0;
            // }
        }

        public void SetOverlayVisible(bool value)
        {
            overlay.gameObject.SetActive(value);
        }

        public void SetOverlay3dObject(bool value)
        {
            overlay.localPosition = new Vector3(0, 0, value ? 0 : 300);
        }

        private async void ShowHandWithDelay(Transform target)
        {
            await UniTask.Delay(500);
            hand.position = target.position;
            hand.localPosition = new Vector3(hand.localPosition.x, hand.localPosition.y, 0);
        }
    }
}