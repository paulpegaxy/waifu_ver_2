using Game.Model;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ShopItemSubscription : MonoBehaviour
    {
        [SerializeField] private Image imageIcon;
        [SerializeField] private TMP_Text textValue;

        public void SetData(ModelApiItemData data)
        {
            imageIcon.sprite = ControllerSprite.Instance.GetResourceIcon(data.IdResource);
            imageIcon.SetNativeSize();

            // if (data.id == TypeResource.Gold)
            // {
            //     textValue.text = data.Quantity.ToLetter();
            // }
            // else
            // {
            //     textValue.text = data.Quantity.ToString();
            // }
        }
    }
}