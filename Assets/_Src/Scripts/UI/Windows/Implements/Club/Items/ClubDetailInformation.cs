using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClubDetailInformation : MonoBehaviour
{
    [SerializeField] private TMP_Text txtScore;
    [SerializeField] private TMP_Text txtHint;
    [SerializeField] private TMP_Text txtMember;
    [SerializeField] private Image imgIcon;

    public void LoadData(ModelApiClubData data)
    {
        txtScore.text = data.TotalPointParse.ToLetter();
        txtMember.text = data.total_members.ToString();
    }
}
