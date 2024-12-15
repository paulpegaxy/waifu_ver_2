// Author: 
// Created Date: 25/07/2024
// Update Time: 25/07

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemRanking : MonoBehaviour
    {
        [SerializeField] ItemAvatar itemAvatar;
        [SerializeField] private TMP_Text textRank;
        [SerializeField] private Image imageBackground;
        [SerializeField] private Image imageRank;
        [SerializeField] private bool isAutoSetColor = true;

        public void SetData(int rank, string name)
        {
            textRank.text = rank.ToString();
            itemAvatar.SetNameAvatar(name);

            ExtensionImage.LoadRankIcon(imageRank, rank);
        }

        public void SetData(int rank, string name, bool isMyRank = false)
        {
            textRank.text = rank.ToString();
            itemAvatar.SetNameAvatar(name);

            ExtensionImage.LoadRankIcon(imageRank, rank);

            if (!isAutoSetColor)
                return;
            if (isMyRank)
            {
                imageBackground.color = GameUtils.GetColor("#B6F6FB");
            }
            else
            {
                imageBackground.color = GameUtils.GetColor("#FFFFFF");
            }
        }

        public void SetData(string rank, string name, bool isMyRank = false)
        {
            if (int.TryParse(rank, out var rankInt))
            {
                SetData(rankInt, name, isMyRank);
            }
            else
            {
                textRank.text = rank;
                itemAvatar.SetNameAvatar(name);
                ExtensionImage.LoadRankIcon(imageRank, 4);
                if (isAutoSetColor)
                    imageBackground.color = GameUtils.GetColor("#FFFFFF");
            }
        }
    }
}