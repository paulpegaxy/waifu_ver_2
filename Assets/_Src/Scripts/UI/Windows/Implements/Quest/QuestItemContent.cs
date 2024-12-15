using System;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Template.Defines;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class QuestItemContent : MonoBehaviour
    {
        [SerializeField] private ItemReward itemReward;
        [SerializeField] private QuestProgress questProgress;
        [SerializeField] private TMP_Text textDescription;
        [SerializeField] private TMP_Text txtBtnGo;
        [SerializeField] private UIButton buttonGo;
        [SerializeField] private UIButton buttonClaim;
        [SerializeField] private GameObject objectWaiting;
        [SerializeField] private GameObject objectClaimed;
        
        private float _duration;
        private ModelApiQuestData _data;
        
        private Action<ModelApiQuestData> OnGo;
        private Action<ModelApiQuestData,Vector3> OnClaim;

        private void OnEnable()
        {
            buttonGo.onClickEvent.AddListener(OnGoClick);
            buttonClaim.onClickEvent.AddListener(OnClaimClick);
        }

        private void OnDisable()
        {
            buttonGo.onClickEvent.RemoveListener(OnGoClick);
            buttonClaim.onClickEvent.RemoveListener(OnClaimClick);
        }

        private void Update()
        {
            if (_duration > 0)
            {
                _duration -= Time.deltaTime;
                if (_duration <= 0)
                {
                    _duration = 0;
                    _data.ReadyToClaim();

                    // SetData(_data);
                }
            }
        }

        private void OnGoClick()
        {
            if (!objectWaiting.activeSelf)
                OnGo?.Invoke(_data);
        }

        private void OnClaimClick()
        {
            OnClaim?.Invoke(_data, itemReward.transform.position);
        }

        public void SetData(ModelApiQuestData quest, Action<ModelApiQuestData> OnGo, Action<ModelApiQuestData, Vector3> OnClaim)
        {
            _duration = quest.end_time - ServiceTime.CurrentUnixTime;
            if (!quest.claimed && quest.end_time > 0 && _duration <= 0)
            {
                quest.ReadyToClaim();
            }


            questProgress.SetData(quest);

            itemReward.SetData(quest.items[0].IdResource, quest.items[0].QuantityParse);
            itemReward.SetOverlay(quest.claimed);

            if (quest.Type == QuestType.TonCheckInVerify)
            {
                txtBtnGo.text = Localization.Get(TextId.Common_LbVerify);
            }
            else
            {
                txtBtnGo.text = Localization.Get(TextId.Common_GoTo);
            }

            textDescription.text = quest.description;

#if UNITY_EDITOR
            textDescription.text += " - " + quest.id;
#endif
            // textDescription.text = ExtensionEnum.ToQuestLocalize(quest.id);

            txtBtnGo.gameObject.SetActive(_duration <= 0);
            objectWaiting.SetActive(!txtBtnGo.gameObject.activeSelf);

            buttonClaim.gameObject.SetActive(quest.can_claim);
            buttonGo.gameObject.SetActive(!quest.claimed && !quest.can_claim);
            objectClaimed.SetActive(quest.claimed);

            _data = quest;
            
            this.OnGo = OnGo;
            this.OnClaim = OnClaim;
        }
    }
}