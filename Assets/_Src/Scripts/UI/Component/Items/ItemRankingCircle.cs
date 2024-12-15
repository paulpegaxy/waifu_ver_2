using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemRankingCircle : MonoBehaviour
{
    [SerializeField] private Image imgHolderRank;
    [SerializeField] private TMP_Text txtRank;

    public void SetData(int rankPos)
    {
        txtRank.text = rankPos.ToString();
        // var commonRank = 4;
        // if (rankPos < commonRank)
        // {
        //     imgHolderRank.sprite = ControllerSprite.Instance.GetRankCircleBg(rankPos);
        // }
        // else
        // {
        //     imgHolderRank.sprite = ControllerSprite.Instance.GetRankCircleBg(commonRank);
        // }
        ExtensionImage.LoadRankIcon(imgHolderRank, rankPos, true);
    }
}
