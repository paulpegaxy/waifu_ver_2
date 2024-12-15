using System;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;

namespace Game.UI
{
    public class PartnerMergePalFilterType : AItemFilter<TypeFilterPartner>
    {
        [SerializeField] private GameObject toggleRanking;
        
        public void ActiveRankingFilter(bool isActive)
        {
            toggleRanking.SetActive(isActive);
        }
    }

    public enum TypeFilterPartner
    {
        Collab,
        Quest,
        Ranking
    }
}