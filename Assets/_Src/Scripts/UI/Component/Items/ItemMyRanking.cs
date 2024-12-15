using System;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Model;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
    public class ItemMyRanking : GameplayListener
    {
        [SerializeField] private Image iconRank;
        [SerializeField] private TMP_Text txtRank;
        [SerializeField] private UIButton btnRank;
        
        private ModelApiGameInfo _data;

        protected override void OnEnable()
        {
            base.OnEnable();
            btnRank.onClickEvent.AddListener(OnClickRanking);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            btnRank.onClickEvent.RemoveListener(OnClickRanking);
        }

        protected override void OnGameInfoChanged(ModelApiGameInfo gameInfo)
        {
            _data = gameInfo;
            // txtRank.text = gameInfo.higheshLeagueCharacter.ToString();
            // iconRank.sprite = ControllerSprite.Instance.GetLeagueIcon(gameInfo.higheshLeagueCharacter);
        }

        private void OnClickRanking()
        {
            this.PostEvent(TypeGameEvent.Club, new ModelClubFilter()
            {
                FilterType = FilterType.Personal,
                FilterTimeType = FilterTimeType.Lifetime,
                typeLeagueIndex =  (int) _data.CurrentCharRank
            });

            Signal.Send(StreamId.UI.Club);
        }
    }
}