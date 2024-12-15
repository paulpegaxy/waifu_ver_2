using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using Game.Model;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
    public class QuestCellViewContentQuest : ESCellView<ModelQuestCellView>
    {
        [SerializeField] private ItemReward itemReward;
        [SerializeField] private QuestProgress questProgress;
        [SerializeField] private TMP_Text textDescription;
        [SerializeField] private TMP_Text txtBtnGo;
        [SerializeField] private UIButton buttonGo;
        [SerializeField] private UIButton buttonClaim;
        [SerializeField] private GameObject objectWaiting;
        [SerializeField] private GameObject objectClaimed;

        public static Action<ModelQuestCellViewContentQuest> OnGo;
        public static Action<ModelQuestCellViewContentQuest, Vector3> OnClaim;

        private ModelQuestCellViewContentQuest _data;
        private float _duration;

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
                    _data.Quest.ReadyToClaim();

                    SetData(_data);
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

        public override async void SetData(ModelQuestCellView model)
        {
            var data = model as ModelQuestCellViewContentQuest;
            var quest = data.Quest;

            _duration = quest.end_time - ServiceTime.CurrentUnixTime;
            if (!quest.claimed && quest.end_time > 0 && _duration <= 0)
            {
                quest.ReadyToClaim();
            }


            questProgress.SetData(quest);

            itemReward.SetData(quest.items[0].IdResource, quest.items[0].QuantityParse);
            itemReward.SetOverlay(quest.claimed);

            switch (quest.Type)
            {
                case QuestType.CheckInJackpot:
                    txtBtnGo.text = Localization.Get(TextId.Quest_CheckIn);
                    break;
                case QuestType.TonCheckInVerify:
                    txtBtnGo.text = Localization.Get(TextId.Common_LbVerify);
                    break;
                default:
                    txtBtnGo.text = Localization.Get(TextId.Common_GoTo);
                    break;
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

            _data = data;
        }
    }
}
