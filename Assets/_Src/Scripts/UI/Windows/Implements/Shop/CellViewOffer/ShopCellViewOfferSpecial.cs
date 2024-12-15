using UnityEngine;
using TMPro;
using Game.Model;
using Game.Runtime;

namespace Game.UI
{
    public class ShopCellViewOfferSpecial : ShopCellViewOffer
    {
        [SerializeField] private ItemTimer timerDuration;
        [SerializeField] private TMP_Text textLimit;

        private void OnEnable()
        {
            ModelApiShop.OnChanged += OnShopChanged;
        }

        private void OnDisable()
        {
            ModelApiShop.OnChanged -= OnShopChanged;
        }

        private void OnShopChanged(ModelApiShop data)
        {
            SetData(Data);
        }

        protected override void SetData(ModelApiShopData data)
        {
            base.SetData(data);
            textLimit.text = $"{Localization.Get(TextId.Shop_Limit)}: {data.purchased_count}/{data.limit}";

            // var duration = data.end_time - ServiceTime.CurrentUnixTime;
            // duration = (long)Mathf.Max(duration, 0);
            // timerDuration.SetDuration(duration);
        }
    }
}