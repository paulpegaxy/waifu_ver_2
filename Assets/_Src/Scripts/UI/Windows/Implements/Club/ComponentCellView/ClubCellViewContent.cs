using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Game.UI;
using Template.Defines;
using TMPro;
using UnityEngine;

public class ClubCellViewContent : ESCellView<ModelClubCellView>
{
    [SerializeField] private ItemRanking itemRanking;
    [SerializeField] private TMP_Text textName;
    [SerializeField] private TMP_Text txtMember;
    [SerializeField] private GameObject objectYour;
    [SerializeField] private UIButton buttonView;
    
    private ModelApiLeaderboardClub _data;
    
    private void OnEnable()
    {
        buttonView.onClickEvent.AddListener(OnView);
    }

    private void OnDisable()
    {
        buttonView.onClickEvent.RemoveListener(OnView);
    }
    
    public override void SetData(ModelClubCellView model)
    {
        var data = model as ModelClubCellViewContenMain;
        var leaderboard = data.LeaderboardData;

        itemRanking.SetData(leaderboard.index, leaderboard.name);
        textName.text = leaderboard.name;

        txtMember.text = leaderboard.total_members.ToFormat();

        var isClub = data.Filter.FilterType == FilterType.Club;
        objectYour.SetActive(IsYourClub(data));
        buttonView.interactable = isClub; 

        _data = leaderboard;
    }
    
    private void OnView()
    {
        this.PostEvent(TypeGameEvent.ClubDetail, _data.id);
        Signal.Send(StreamId.UI.ClubDetail);
    }
    
    private bool IsYourClub(ModelClubCellViewContenMain data)
    {
        var leaderboard = data.LeaderboardData;
        var isClub = data.Filter.FilterType == FilterType.Club;

        if (!isClub || data.UserInfo == null || data.UserInfo.Club == null) return false;
        return data.UserInfo.Club.id == leaderboard.id;
    }
}
