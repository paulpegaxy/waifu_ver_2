// Author: 
// Created Date: 26/07/2024
// Update Time: 26/07

using System;
using BreakInfinity;

namespace Game.Model
{
    [Serializable]
    public class ModelApiCommonSummary
    {
        public int total_user;
        public int total_premium_users;
        public int total_login_users;
        public int total_online_users;
        public string total_point_created;

        public BigDouble TotalPointCreatedParse => BigDouble.Parse(total_point_created);
    }
}