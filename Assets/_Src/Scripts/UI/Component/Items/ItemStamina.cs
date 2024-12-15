// Author: 
// Created Date: 22/07/2024
// Update Time: 22/07

using BreakInfinity;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class ItemStamina : MonoBehaviour
    {
        [SerializeField] private TMP_Text txtStamina;

        protected void OnEnable()
        {
            ControllerResource.OnChanged += OnChanged;
            var apiGame = FactoryApi.Get<ApiGame>().Data.Info;
            if (apiGame==null)
                return;

            txtStamina.text = $"{apiGame.stamina}/{apiGame.stamina_max}";
        }

        protected void OnDisable()
        {
            ControllerResource.OnChanged -= OnChanged;
        }

        private void OnChanged(TypeResource type, BigDouble oldAmount, BigDouble newAmount)
        {
            if (type != TypeResource.ExpWaifu) return;
            SetAmount(newAmount);
        }

        private void SetAmount(BigDouble amount)
        {
            var maxStamina = FactoryApi.Get<ApiGame>().Data.Info.stamina_max;
            if (amount > maxStamina)
                amount = maxStamina;
            if (amount < 0)
            {
                amount = 0;
            }

            txtStamina.text = $"{amount}/{maxStamina}";
        }
    }
}