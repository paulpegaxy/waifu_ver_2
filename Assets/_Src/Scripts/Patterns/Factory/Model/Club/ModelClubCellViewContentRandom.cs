using System;
using Template.Defines;

namespace Game.Model
{
    [Serializable]
    public class ModelClubCellViewContentRandom : ModelClubCellView
    {
        public ModelApiClubData Club;

        public ModelClubCellViewContentRandom()
        {
            Type = TypeClubCellView.ContentRandom;
        }
    }
}
