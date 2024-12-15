using UnityEngine;
using Game.Model;
using TMPro;
using Game.Runtime;

namespace Game.UI
{
    public class MainGenerate : GameplayListener
    {
        [SerializeField] private Transform pivot;

        protected override void OnGameInfoChanged(ModelApiGameInfo gameInfo)
        {

        }
    }
}