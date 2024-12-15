using UnityEngine;
using TMPro;
using Game.Runtime;
using Game.Model;

namespace Game.UI
{
    public class ItemLifetimeCurrency : GameplayListener
    {
        [SerializeField] private TMP_Text textAmount;
        
        protected override void OnGameInfoChanged(ModelApiGameInfo gameInfo)
        {
            textAmount.text = gameInfo.PointAllTimeParse.ToLetter();
        }
    }
}