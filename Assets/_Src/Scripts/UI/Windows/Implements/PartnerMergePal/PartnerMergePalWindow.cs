
using System;
using Cysharp.Threading.Tasks;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Sirenix.Utilities;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public class PartnerMergePalWindow : UIWindow
    {
        [SerializeField] private APartnerMergePalPanel[] arrPanel;
        [SerializeField] private MainWindowAction typePartner;
        
        private MainWindowAction _partnerType;

        private TypeFilterPartner _filterType = TypeFilterPartner.Collab;
        
        protected override void OnEnabled()
        {
            _filterType = TypeFilterPartner.Collab;
            PartnerMergePalFilterType.OnChanged+= ShowPanel;
            ModelApiEvent.OnChanged += OnRefreshData;
            QuestExecute.OnRefresh += OnRefresh;
            
            if (typePartner==MainWindowAction.None)
            {
                _partnerType = this.GetEventData<TypeGameEvent,MainWindowAction>(TypeGameEvent.Partner, true);
            }
            else
            {
                _partnerType = typePartner;
            }

            arrPanel.ForEach(x => x.Init(_partnerType));

            CheckEmptyFilter();
            
            ShowPanel(_filterType);
        }

        protected override void OnDisabled()
        {
            PartnerMergePalFilterType.OnChanged -= ShowPanel;
            ModelApiEvent.OnChanged -= OnRefreshData;
            QuestExecute.OnRefresh -= OnRefresh;
        }

        private void CheckEmptyFilter()
        {
            var eventData = FactoryApi.Get<ApiEvent>().Data.GetEventByType(_partnerType);
            if (eventData.empty_filter)
            {
                _filterType = TypeFilterPartner.Quest;
            }
            else
            {
                _filterType = TypeFilterPartner.Collab;
            }
        }

        private void OnRefreshData(ModelApiEvent data)
        {
            ShowPanel(_filterType);
        }
        
        private void OnRefresh()
        {
            ShowPanel(_filterType);
        }

        private void ShowPanel(TypeFilterPartner type)
        {
            _filterType = type;
            arrPanel.ForEach(x => x.Show(type));
        }
    }
}
