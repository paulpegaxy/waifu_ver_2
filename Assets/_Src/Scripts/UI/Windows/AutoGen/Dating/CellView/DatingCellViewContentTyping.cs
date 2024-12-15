using Game.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class DatingCellViewContentTyping : ESCellView<ModelDatingCellView>
    {
        [SerializeField] private Image imgAva;
        
        public override void SetData(ModelDatingCellView model)
        {
            if (model is ModelDatingCellViewContentTyping data)
            {
                imgAva.sprite = data.SprAvatar;
            }
        }
    }
}