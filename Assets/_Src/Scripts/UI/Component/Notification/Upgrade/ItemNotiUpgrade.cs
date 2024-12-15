using System;
using System.Collections.Generic;
using Game.Model;
using Game.Runtime;

namespace Game.UI.Upgrade
{
    public class ItemNotiUpgrade : ItemNotificationGeneric<List<ModelApiIdleEarnUpgrade>>
    {
        protected override void OnEnabled()
        {
            SetData(FactoryApi.Get<ApiUpgrade>().Data.upgrade);
            ModelApiGameInfo.OnChanged += OnGameInfoChanged;
            ModelApiUpgradeInfo.OnChanged += OnUpgradeInfoChanged;
        }

        protected override void OnDisabled()
        {
            ModelApiGameInfo.OnChanged -= OnGameInfoChanged;
            ModelApiUpgradeInfo.OnChanged -= OnUpgradeInfoChanged;
        }
        
        private void OnGameInfoChanged(ModelApiGameInfo data)
        {
            Refresh();
        }
        
        private void OnUpgradeInfoChanged(ModelApiUpgradeInfo data)
        {
            SetData(data.upgrade);
        }

        protected override bool IsValid()
        {
            if (_data == null) return false;

            var currValue = FactoryApi.Get<ApiGame>().Data.Info.PointParse;

            return _data.Find(x => currValue >= x.next.CostParse) != null;
        }
    }
}