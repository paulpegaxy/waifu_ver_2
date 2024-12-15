using Game.Extensions;
using UnityEngine;
using Game.Runtime;
using Game.Model;
using Template.Defines;
using Template.Runtime;

namespace Game.UI
{
    public class QuestCellViewContentAchievement : ESCellView<ModelQuestCellView>
    {
        [SerializeField] private AchievementScroller scroller;

        private void OnEnable()
        {
            AchievementCellView.OnClaim += OnClaim;
        }

        private void OnDisable()
        {
            AchievementCellView.OnClaim -= OnClaim;
        }

        private async void OnClaim(ModelApiQuestData data, Vector3 position)
        {
            var achievements = scroller.GetData();
            var index = achievements.FindIndex(x => x.id == data.id);
            if (index < 0) return;

            var apiQuest = FactoryApi.Get<ApiQuest>();
            await apiQuest.Claim(data.id);

            foreach (var item in data.items)
            {
                ControllerResource.Add(item.IdResource, item.QuantityParse);
            }

            data.Claim();
            apiQuest.Data.Sync(data);

            scroller.ReloadData();

            ControllerUI.Instance.Spawn(TypeResource.Berry, position, 20);

            this.PostEvent(TypeGameEvent.ClaimAchievementSuccess);
            ControllerPopup.ShowToastSuccess(Localization.Get(TextId.Common_Claimed));
        }

        public override void SetData(ModelQuestCellView model)
        {
            var data = model as ModelQuestCellViewContentAchievement;
            scroller.SetData(data.Quests);

            foreach (var item in data.Quests)
            {
                if (item.can_claim && !item.claimed)
                {
                    scroller.JumpToDataIndex(data.Quests.IndexOf(item));
                    break;
                }
            }
        }
    }
}
