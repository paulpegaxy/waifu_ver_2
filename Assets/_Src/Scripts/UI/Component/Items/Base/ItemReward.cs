using BreakInfinity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Template.Defines;

namespace Game.UI
{
    public class ItemReward : MonoBehaviour
    {
        [SerializeField] private Image imageBackground;
        [SerializeField] private Image imageIcon;
        [SerializeField] private TMP_Text textAmount;
        [SerializeField] private GameObject objectOverlay;

        private void Awake()
        {
            objectOverlay.SetActive(false);
        }

        public void SetData(TypeResource type, BigDouble amount)
        {
            imageIcon.sprite = ControllerSprite.Instance.GetResourceIcon(type);
            if (type == TypeResource.HeartPoint)
                textAmount.text = amount.ToLetter();
            else
                textAmount.text = amount.ToString();
        }

        public void SetOverlay(bool active)
        {
            objectOverlay.SetActive(active);
        }
    }
}