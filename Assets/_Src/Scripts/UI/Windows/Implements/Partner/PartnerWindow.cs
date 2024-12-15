using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Game.Runtime;
using Game.Model;
using Game.Defines;
using Game.Core;
using Game.Extensions;
using Template.Defines;

namespace Game.UI
{
	public class PartnerWindow : UIWindow
	{
		[SerializeField] private PartnerScroller scroller;
		[SerializeField] private string id;

		protected string _id;

		protected override void OnEnabled()
		{
			ModelApiShop.OnChanged += OnChanged;
			ModelApiEvent.OnChanged += OnChanged;
			ModelApiQuest.OnChanged += OnChanged;
			QuestExecute.OnRefresh += OnRefresh;

			if (string.IsNullOrEmpty(id))
			{
				var data = this.GetEventData<TypeGameEvent,MainWindowAction>(TypeGameEvent.Partner, true);
				_id = data.ToString().PascalToSnake().Replace("partner_", "");
			}
			else
			{
				_id = id;
			}

			Fetch();
		}

		protected override void OnDisabled()
		{
			ModelApiShop.OnChanged -= OnChanged;
			ModelApiEvent.OnChanged -= OnChanged;
			ModelApiQuest.OnChanged -= OnChanged;
			QuestExecute.OnRefresh -= OnRefresh;
		}

		private void OnChanged(ModelApiShop data)
		{
			scroller.ReloadData();
		}

		private void OnChanged(ModelApiEvent data)
		{
			scroller.ReloadData();
		}

		private void OnChanged(ModelApiQuest data)
		{
			scroller.ReloadData();
		}

		private void OnRefresh()
		{
			Fetch();
		}

		protected virtual async void Fetch()
		{
			ControllerPopup.SetApiLoading(true);
			try
			{
				var apiShop = FactoryApi.Get<ApiShop>();
				var items = await apiShop.Get();

				var apiQuest = FactoryApi.Get<ApiQuest>();
				var data = await apiQuest.Get();

				Process();
			}
			catch (Exception e)
			{
				e.ShowError();
			}
			ControllerPopup.SetApiLoading(false);
		}

		protected virtual void Process()
		{
			var data = new List<ModelPartnerCellView>();
			var apiEvent = FactoryApi.Get<ApiEvent>();
			var apiuser = FactoryApi.Get<ApiUser>();
			var config = apiEvent.Data.GetConfig(_id);
			var isPartner = config.id == apiuser.Data.User.user_from;

			if (config == null) return;

			data.Add(new ModelPartnerCellViewBanner() { Config = config });
			foreach (var module in config.modules_order)
			{
				if (!isPartner && config.private_modules.Contains(module)) continue;

				if (Enum.TryParse<PartnerCellViewType>(module, out var type))
				{
					var items = GetModule(type, config);
					var title = GetTitle(type);

					if (!string.IsNullOrEmpty(title) && items.Count > 0)
					{
						data.Add(new ModelPartnerCellViewHeader() { Title = GetTitle(type) });
					}

					if (items.Count > 0)
					{
						data.AddRange(items);
					}
				}
			}

			scroller.SetData(data);
		}

		protected string GetTitle(PartnerCellViewType type)
		{
			return type switch
			{
				PartnerCellViewType.Bonus => Localization.Get(TextId.Common_Reward),
				PartnerCellViewType.Quest => Localization.Get(TextId.Quest_DailyTask),
				PartnerCellViewType.Package => Localization.Get(TextId.Shop_SpecialOffer).Replace("\n", " "),
				PartnerCellViewType.PackageCollab => Localization.Get(TextId.Shop_CollabPack),
				PartnerCellViewType.PackageEvent => Localization.Get(TextId.Shop_EventPack),
				_ => string.Empty,
			};
		}

		protected List<ModelPartnerCellView> GetModule(PartnerCellViewType type, ModelApiEventConfig config)
		{
			var apiQuest = FactoryApi.Get<ApiQuest>();
			var apiShop = FactoryApi.Get<ApiShop>();
			var data = new List<ModelPartnerCellView>();

			switch (type)
			{
				case PartnerCellViewType.Bonus:
					data.Add(new ModelPartnerCellViewBonus() { Config = config });
					break;

				case PartnerCellViewType.Quest:
					var quests = apiQuest.Data.Quest.FindAll(x => x.category == config.quest_type);
					if (quests.Count > 0)
					{
						foreach (var quest in quests)
						{
							data.Add(new ModelPartnerCellViewQuest()
							{
								Quest = quest
							});
						}
					}
					break;

				case PartnerCellViewType.Package:
					var shopItems = apiShop.Data.Shop.FindAll(x => x.pack == config.iap_offer);
					for (int i = 0; i < shopItems.Count; i++)
					{
						data.Add(new ModelPartnerCellViewPackage()
						{
							Index = i + 1,
							ShopData = shopItems[i]
						});
					}
					break;

				case PartnerCellViewType.PackageCollab:
					var collabItem = apiShop.Data.Shop.Find(x => x.id == config.iap_collab);
					if (collabItem != null)
					{
						data.Add(new ModelPartnerCellViewPackageCollab()
						{
							ShopData = collabItem
						});
					}
					break;

				case PartnerCellViewType.PackageEvent:
					var eventItem = apiShop.Data.Shop.Find(x => x.id == config.iap_collab);
					if (eventItem != null)
					{
						data.Add(new ModelPartnerCellViewPackageEvent()
						{
							ShopData = eventItem
						});
					}
					break;

				case PartnerCellViewType.Checkin:
					var apiEvent = FactoryApi.Get<ApiEvent>();
					apiEvent.CheckIn(config.id).Forget();
					data.Add(new ModelPartnerCellViewCheckin() { Config = config });
					break;

				case PartnerCellViewType.Claim:
					data.Add(new ModelPartnerCellViewClaim() { Config = config });
					break;
			}

			return data;
		}
	}
}