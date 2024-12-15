using System;
using System.Collections.Generic;

namespace Game.Model
{
    [Serializable]
    public class ModelApiUser : ModelApiNotification<ModelApiUser>
    {
        public ModelApiUserData User;
        public ModelApiUserShopData Shop;
        public ModelApiUserGameData Game;
        public ModelApiClubData Club;
        public List<ModelApiUserSubscriptionData> Subscriptions;

        public override void Notification()
        {
            OnChanged?.Invoke(this);
        }

        public void AddCheatTagPartner()
        {
            //test
            if (!User.tags.Contains("yggplay"))
                User.tags.Add("yggplay");
            Notification();
        }
        
        public bool IsHaveSubscription(string id)
        {
            return Subscriptions.Exists(x => x.id == id);
        }
    }
}