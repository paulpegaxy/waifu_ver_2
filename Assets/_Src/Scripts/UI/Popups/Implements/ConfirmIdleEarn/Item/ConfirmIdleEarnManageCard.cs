// Author: ad   -
// Created: 08/08/2024  : : 01:08
// DateUpdate: 08/08/2024

using BreakInfinity;
using UnityEngine;

namespace Game.UI
{
    public class ConfirmIdleEarnManageCard : MonoBehaviour
    {
        [SerializeField] private IdleEarnCardInfo cardInfo;

        public void LoadData(string id, int level, BigDouble profitPerHour)
        {
            cardInfo.SetData(new DataIdleEarnUpgradeItem()
            {
                id = id,
                level = level,
                profitPerHour = profitPerHour
            });
        }
    }
}