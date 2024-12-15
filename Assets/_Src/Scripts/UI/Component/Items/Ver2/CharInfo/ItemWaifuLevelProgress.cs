// Author: ad   -
// Created: 14/12/2024  : : 15:12
// DateUpdate: 14/12/2024

using Game.Model;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemWaifuLevelProgress : MonoBehaviour
    {
        [SerializeField] private Image imgSliderExp;
        [SerializeField] private TMP_Text txtExp;
        [SerializeField] private Color[] arrColorExp;

        public void SetData(ModelApiEntityConfig entityConfig)
        {
            var dataExp = FactoryApi.Get<ApiEntity>().Data.GetExpDisplay(entityConfig);
            txtExp.text = entityConfig.exp + "/" + dataExp.total_exp_at_level;
            var sliderValue = dataExp.GetSliderValue();
            imgSliderExp.fillAmount = sliderValue;
            imgSliderExp.color = arrColorExp[sliderValue > 0.5f ? 1 : 0];
        }
    }
}