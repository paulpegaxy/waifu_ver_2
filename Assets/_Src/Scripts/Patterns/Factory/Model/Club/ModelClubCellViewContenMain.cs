using Template.Defines;

namespace Game.Model
{
    public class ModelClubCellViewContenMain : ModelClubCellView
    {
        public ModelClubFilter Filter;
        public ModelApiLeaderboardClub LeaderboardData;
        public ModelApiUser UserInfo;
        public bool IsMine;

        public ModelClubCellViewContenMain()
        {
            Type = TypeClubCellView.ContentMain;
        }
    }
}