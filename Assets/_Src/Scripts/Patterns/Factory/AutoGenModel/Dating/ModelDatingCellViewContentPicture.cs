using UnityEngine;

namespace Game.Model
{
    public class ModelDatingCellViewContentPicture : ModelDatingCellView
    {
        public ModelApiEntityConfig EntityConfig;
        public string PictureMessage;
        public Sprite SprAva;
        
        public ModelDatingCellViewContentPicture()
        {
            Type = DatingCellViewType.ContentPicture;
        }
    }
}