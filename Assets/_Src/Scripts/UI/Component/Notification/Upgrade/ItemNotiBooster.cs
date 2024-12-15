using Game.Model;
using Game.Runtime;

namespace Game.UI
{
    public class ItemNotiBooster : ItemNotificationGeneric<ModelApiUpgradeInfo>
    {
        protected override void OnEnabled()
        {
            SetData(FactoryApi.Get<ApiUpgrade>().Data);
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
            SetData(data);
        }
        
        protected override bool IsValid()
        {
            if (_data == null) return false;

            var currValue = FactoryApi.Get<ApiGame>().Data.Info.PointParse;


            if (_data.next.point_tap.CostParse <= currValue)
                return true;

            if (_data.next.stamina_max.CostParse <= currValue)
                return true;

            if (_data.current.charge_stamina > 0)
            {
                if (_data.next.next_charge_stamina_wait_time <= 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}