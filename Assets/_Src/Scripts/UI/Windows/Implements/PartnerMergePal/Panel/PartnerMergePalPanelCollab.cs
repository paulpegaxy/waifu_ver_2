using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Model;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
    public class PartnerMergePalPanelCollab : APartnerMergePalPanel
    {
        protected override async UniTask OnProcessLoadPartner(MainWindowAction type)
        {
            // UnityEngine.Debug.LogError("HAVE PARTNER RANKING : " + ParterData.IsHaveRanking);
            ListData.Add(new ModelPartnerMergePalCellViewHeaderCollab()
            {
                FilterType = TypeFilterPartner.Collab,
                eventConfig = ParterData,
                TypePartner = type,
                IsHaveRankingFilter = ParterData.IsHaveRanking
            });

            ProcessShopBundle(type);

            mergePalScroller.SetData(ListData);
        }

        private void ProcessShopBundle(MainWindowAction type)
        {
            var apiShop = FactoryApi.Get<ApiShop>();
            if (apiShop.Data == null)
                return;

            bool isHavePrivate = SpecialExtensionGame.IsMatchTagEvent(ParterData);
            if (isHavePrivate)
            {
                var list = apiShop.Data.GetListShopItemBasedEvent(ParterData.PrivatePartnerId);
                for (var i = 0; i < list.Count; i++)
                {
                    var ele = list[i];
                    ListData.Add(new ModelPartnerMergePalCellViewContentBundleOffer()
                    {
                        Data = ele
                    });
                }
                // list.ForEach(x=>UnityEngine.Debug.LogError("PRIVATE ITEM : " + x.id));
            }

            var shopList = apiShop.Data.GetListShopItemBasedEvent(ParterData.id);

            // if (shopList.Count > 0)
            // {
            //     ListData.Add(new ModelPartnerMergePalCellViewHeaderBundle());
            // }

            var itemBotTap = shopList.FirstOrDefault(x =>
                x.GetPackType() == TypeShopPack.TapBotPrimePack);
            if (itemBotTap != null)
            {
                ListData.Add(new ModelPartnerMergePalCellViewContentBundleBotTap()
                {
                    Data = itemBotTap
                });
            }

            var groupItemTimelapse = shopList.Where(x =>
                x.GetPackType() == TypeShopPack.TimeLapse).ToList();
            if (groupItemTimelapse.Count > 0)
            {
                ListData.Add(new ModelPartnerMergePalCellViewContentBundleTimelapse()
                {
                    ListItemData = groupItemTimelapse
                });
            }


        }
    }
}