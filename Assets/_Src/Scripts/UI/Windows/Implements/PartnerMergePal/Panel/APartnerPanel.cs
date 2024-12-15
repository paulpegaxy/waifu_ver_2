using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Model;
using Game.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.UI
{
    public abstract class APartnerMergePalPanel : BaseWindowPanel<TypeFilterPartner,AModelPartnerMergePalCellView>
    {
        [SerializeField] private TypeFilterPartner typePanel; 
        [SerializeField] protected PartnerMergePalScroller mergePalScroller;

        private MainWindowAction _typePartner;
        
        protected bool IsWaitingLoadData;
        protected ModelApiEventConfig ParterData;

        protected  override void SetData()
        {
        }

        public void Init(MainWindowAction type)
        {
            _typePartner = type;
            ParterData = FactoryApi.Get<ApiEvent>().Data.GetEventByType(type);
        }
        
        protected override bool IsThisPanel(TypeFilterPartner type)
        {
            return typePanel == type;
        }

        protected override async UniTask OnLoadData()
        {
            await OnProcessLoadPartner(_typePartner);
        }

        protected abstract UniTask OnProcessLoadPartner(MainWindowAction type);
    }
}