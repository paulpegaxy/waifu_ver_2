using System;
using System.Collections.Generic;

namespace Game.Model
{
    [Serializable]
    public class ModelApiClub : ModelApiNotification<ModelApiClub>
    {
        public ModelApiClubAll Clubs;
        public List<ModelApiClubBoostInfo> BoostPrices;

        public int GetRank(int id)
        {
            return Clubs.data.FindIndex(x => x.id == id) + 1;
        }

        public override void Notification()
        {
            OnChanged?.Invoke(this);
        }
    }
}