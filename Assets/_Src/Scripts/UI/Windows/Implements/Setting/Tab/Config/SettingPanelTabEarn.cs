using System;
using Game.Model;
using Game.Runtime;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class SettingPanelTabEarn : MonoBehaviour
    {
        [SerializeField] private TMP_Text txtSc;
        [SerializeField] private TMP_Text txtHc;

        private void OnEnable()
        {
            var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;
            if (gameInfo == null)
                return;

            txtHc.text = gameInfo.BerryParse.ToLetter();
            txtSc.text = gameInfo.PointParse.ToLetter();
        }
    }
}