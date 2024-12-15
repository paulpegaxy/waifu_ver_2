using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelClubCellViewContentPersonal : ModelClubCellView
    {
        public ModelClubFilter Filter;
        public ModelApiLeaderboardData LeaderboardData;
        public ModelApiUser UserInfo;
        public bool IsMine;

        public ModelClubCellViewContentPersonal()
        {
            Type = TypeClubCellView.ContentPersonal;
        }
    }
}
