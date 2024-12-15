using System;

namespace Game.Model
{
    [Serializable]
    public class ModelApiEventCheckin
    {
        public string id;
        public int days;
        public float ton_reward_amount;
        public int total_users_claimed;
        public int total_users_can_claim;
        public bool is_claimed;
        public bool can_claim;
    }
}