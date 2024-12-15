using System;
using Template.Defines;
using UnityEngine.Serialization;

namespace Game.Model
{
    [Serializable]
    public class ModelBuddyCellViewContentNormal : ModelBuddyCellView
    {
        public BuddyType BuddyType;
        [FormerlySerializedAs("Pal")] public float HardCurrency;
        public float Ton;
        public int CurrentFriend;
        public int MaxFriend;
        public int Commission;
        public float TotalSpend;
        public float MaxSpend;
        public float UnlockPrice;
        public bool IsLocked => CurrentFriend < MaxFriend || TotalSpend < MaxSpend;

        public ModelBuddyCellViewContentNormal()
        {
            Type = BuddyCellViewType.ContentNormal;
        }
    }
}
