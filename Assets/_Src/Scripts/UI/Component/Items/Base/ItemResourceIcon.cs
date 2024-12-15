using Template.Defines;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemResourceIcon : MonoBehaviour
    {
        [SerializeField] private Image imageIcon;

        public void SetData(TypeResource type)
        {
            imageIcon.sprite = ControllerSprite.Instance.GetResourceIcon(type);
        }

        public void SetData(Sprite sprIcon)
        {
            imageIcon.sprite = sprIcon;
        }
    }
}