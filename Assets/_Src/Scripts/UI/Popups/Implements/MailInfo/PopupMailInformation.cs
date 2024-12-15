using System;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using TMPro;
using Game.Model;
using Game.Runtime;
using Game.Defines;
using Template.Defines;
using Template.Runtime;

namespace Game.UI
{
    public class PopupMailInformation : MonoBehaviour
    {
        [SerializeField] private TMP_Text textDescription;
        [SerializeField] private UIButton buttonClaim;
        [SerializeField] private UIButton buttonDelete;
        [SerializeField] private List<ItemReward> itemRewards;

        private ModelApiMailData _data;
        private Dictionary<TypeResource, Vector3> _rewardPosition = new();

        private void Start()
        {
            for (var i = 0; i < _data.rewards.Count; i++)
            {
                _rewardPosition[_data.rewards[i].IdResource] = itemRewards[i].transform.position;
            }

            buttonClaim.onClickEvent.AddListener(OnClaim);
            buttonDelete.onClickEvent.AddListener(OnDelete);
        }

        private void OnDestroy()
        {
            buttonClaim.onClickEvent.RemoveListener(OnClaim);
            buttonDelete.onClickEvent.RemoveListener(OnDelete);
        }

        private async void OnClaim()
        {
            ControllerPopup.SetApiLoading(true);
            try
            {
                var apiMail = FactoryApi.Get<ApiMail>();
                await apiMail.Claim(_data.id);
                await FactoryApi.Get<ApiGame>().GetInfo();

                // foreach (var reward in _data.rewards)
                // {
                    // ControllerUI.Instance.Spawn(reward.IdResource, _rewardPosition[reward.IdResource], 20);
                // }

                apiMail.Data.Claim(_data);
                
                GetComponent<UIPopup>().Hide();
            }
            catch (Exception e)
            {
                e.ShowError();
            }
            ControllerPopup.SetApiLoading(false);
        }

        private async void OnDelete()
        {
            ControllerPopup.SetApiLoading(true);
            try
            {
                var apiMail = FactoryApi.Get<ApiMail>();
                await apiMail.Delete(_data.id);

                apiMail.Data.Delete(_data);
                GetComponent<UIPopup>().Hide();
            }
            catch (Exception e)
            {
                e.ShowError();
            }
            ControllerPopup.SetApiLoading(false);
        }

        public void SetData(ModelApiMailData data)
        {
            for (var i = 0; i < itemRewards.Count; i++)
            {
                if (i < data.rewards.Count)
                {
                    itemRewards[i].SetData(data.rewards[i].IdResource, data.rewards[i].QuantityParse);
                    itemRewards[i].SetOverlay(data.is_claimed);
                }
                itemRewards[i].gameObject.SetActive(i < data.rewards.Count);
            }

            buttonClaim.gameObject.SetActive(!data.is_claimed);
            buttonDelete.gameObject.SetActive(data.is_claimed);
            textDescription.text = data.description;

            _data = data;
        }
    }
}