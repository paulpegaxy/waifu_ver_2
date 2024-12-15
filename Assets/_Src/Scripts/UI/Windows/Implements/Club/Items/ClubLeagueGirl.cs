using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClubLeagueGirl : MonoBehaviour
{
    [SerializeField] private Image imgBannerGirl;
    [SerializeField] private Image imgIconRank;
    [SerializeField] private TMP_Text txtRank;
    [SerializeField] private GameObject objIconLock;

    [SerializeField] private UIButton buttonNext;
    [SerializeField] private UIButton buttonPrev;

    public static Action<int> OnChanged;

    private TypeLeagueCharacter _type = TypeLeagueCharacter.Char_1;
    private TypeLeagueCharacter _maxType;

    private void OnEnable()
    {
        buttonNext.onClickEvent.AddListener(OnNext);
        buttonPrev.onClickEvent.AddListener(OnPrev);
    }

    private void OnDisable()
    {
        buttonNext.onClickEvent.RemoveListener(OnNext);
        buttonPrev.onClickEvent.RemoveListener(OnPrev);
    }

    private void OnNext()
    {
        _type++;
        if (_type > TypeLeagueCharacter.Char_5)
        {
            _type = TypeLeagueCharacter.Char_1;
        }

        SetData(_type);
        OnChanged?.Invoke((int)_type);
    }

    private void OnPrev()
    {
        _type--;
        if (_type < TypeLeagueCharacter.Char_1)
        {
            _type = TypeLeagueCharacter.Char_5;
        }

        SetData(_type);
        OnChanged?.Invoke((int)_type);
    }

    public void SetData(TypeLeagueCharacter type)
    {
        // imgBannerGirl.sprite = ControllerSprite.Instance.GetBannerGirlLeague((int)type);
        imgBannerGirl.LoadSpriteAutoParseAsync("banner_league_" + (int)type);
        imgBannerGirl.SetNativeSize();
        var rankConfig = DBM.Config.rankingConfig.GetRankData(type);
        txtRank.color = rankConfig.rankColor;
        // imgIconRank.sprite = ControllerSprite.Instance.GetLeagueGirlIcon(type);
        imgIconRank.LoadSpriteAutoParseAsync($"league_{(int)type}");
        _type = type;

        var apiGameInfo = FactoryApi.Get<ApiGame>().Data.Info;
        bool isUnlock = ((int)type) <= (int)apiGameInfo.CurrentCharRank;
        SetLock(!isUnlock);
    }

    private void SetLock(bool isLock)
    {
        imgBannerGirl.color = isLock ? Color.black : Color.white;
        objIconLock.SetActive(isLock);
    }
}
