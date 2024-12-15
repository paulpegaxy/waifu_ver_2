using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Game.Model;
using Game.Runtime;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenuClub : MonoBehaviour
{
    [SerializeField] private UIButton btnJoin;
    [SerializeField] private UIButton btnView;

    [SerializeField] private ItemAvatar itemAvatar;
    [SerializeField] private TMP_Text txtClubName;
    [SerializeField] private TMP_Text txtScore;
    [SerializeField] private Image imgRank;


    private void Awake()
    {
        ModelApiUser.OnChanged += OnUserInfoChanged;
        btnJoin.onClickEvent.AddListener(OnJoinClub);
        btnView.onClickEvent.AddListener(OnOpenClub);
    }

    private void OnDestroy()
    {
        ModelApiUser.OnChanged -= OnUserInfoChanged;
        btnJoin.onClickEvent.RemoveListener(OnJoinClub);
        btnView.onClickEvent.RemoveListener(OnOpenClub);
    }

    private void OnEnable()
    {
        Refresh();
    }

    private void OnUserInfoChanged(ModelApiUser userInfo)
    {
        Refresh();
    }

    private void Refresh()
    {
        var apiUser = FactoryApi.Get<ApiUser>();
        var userInfo = apiUser.Data;

        if (userInfo.Club != null)
        {
            txtClubName.text = userInfo.Club.name;
            txtScore.text = userInfo.Club.TotalPointParse.ToLetter();
            // textLeagu.text = userInfo.Club.league.ToString();
            // textLeague.color = userInfo.Club.league.ToColor();
            itemAvatar.SetNameAvatar(userInfo.Club.name);
            // imgRank.sprite = ControllerSprite.Instance.GetLeagueIcon(userInfo.Club.league);

            imgRank.LoadSpriteAutoParseAsync("league_" + userInfo.Club.league);
        }

        btnView.gameObject.SetActive(userInfo.Club != null);
        btnJoin.gameObject.SetActive(userInfo.Club == null);
    }

    private void OnJoinClub()
    {
        Signal.Send(StreamId.UI.ClubRandom);
    }

    private void OnOpenClub()
    {
        var apiUser = FactoryApi.Get<ApiUser>();
        var userInfo = apiUser.Data;

        if (userInfo.Club != null)
        {
            ClubWindow.OpenDetail(userInfo.Club.id);
        }
    }
}
