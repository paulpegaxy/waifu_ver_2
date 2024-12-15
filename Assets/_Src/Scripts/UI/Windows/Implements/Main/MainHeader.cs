using System;
using Cysharp.Threading.Tasks;
using DG.Tweening.Plugins.Core.PathCore;
using Doozy.Runtime.Signals;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;

namespace Game.UI
{
    public class MainHeader : MonoBehaviour
    {
        [SerializeField] private UIButton btnRank;


        private void OnEnable()
        {
            btnRank.onClickEvent.AddListener(OnClickRanking);
        }

        private void OnDisable()
        {
            btnRank.onClickEvent.RemoveListener(OnClickRanking);
        }

        private void OnClickRanking()
        {
            this.PostEvent(TypeGameEvent.Club, new ModelClubFilter()
            {
                FilterType = FilterType.Personal,
                FilterTimeType = FilterTimeType.Lifetime,
                typeLeagueIndex = (int)FactoryApi.Get<ApiGame>().Data.Info.CurrentCharRank
            });

            Signal.Send(StreamId.UI.Club);
        }
    }
}