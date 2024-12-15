using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;
using Game.Runtime;
using Game.Model;

namespace Game.UI
{
    public class ItemUserProfile : AItemCheckStartGame
    {
        [SerializeField] private TMP_Text txtUsername;
        // [SerializeField] private TMP_Text txtLevel;

        protected override void Awake()
        {
            base.Awake();
            // txtLevel.text = "";
            txtUsername.text = "";
        }

        protected override void OnEnabled()
        {
            // OnGameInfoChanged(FactoryApi.Get<ApiGame>().Data.Info);
            ModelApiUser.OnChanged += OnUserChanged;
            // ModelApiGameInfo.OnChanged += OnGameInfoChanged;
        }

        protected override void OnDisabled()
        {
            ModelApiUser.OnChanged -= OnUserChanged;
            // ModelApiGameInfo.OnChanged -= OnGameInfoChanged;
        }

        protected override void OnInit()
        {
            OnUserChanged(FactoryApi.Get<ApiUser>().Data);
        }
        
        // private void OnGameInfoChanged(ModelApiGameInfo gameInfo)
        // {
        //     var currentCharLevel = gameInfo.current_level_girl;
        //     var levelDisplay = currentCharLevel % GameConsts.MAX_LEVEL_PER_CHAR;
        //     txtLevel.text = $"<color=white>LV.{(levelDisplay + 1):00}</color>/{GameConsts.MAX_LEVEL_PER_CHAR}";
        // }

        private void OnUserChanged(ModelApiUser info)
        {
            if (info.User == null) return;
            txtUsername.text = info.User.name;
        }
    }
}