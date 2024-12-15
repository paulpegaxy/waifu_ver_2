using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BreakInfinity;
using Template.Defines;

namespace Game.UI
{
    public class ItemCurrencyColor : MonoBehaviour
    {
        [SerializeField] private TypeResource resourceType;
        [SerializeField] private Image imageBackground;
        [SerializeField] private Image imageIcon;
        [SerializeField] private TMP_Text textAmount;

        private void Awake()
        {
            // imageBackground.color = GetColor();
            imageIcon.sprite = ControllerSprite.Instance.GetResourceIcon(resourceType);
        }

        public void SetAmount(int amount)
        {
            textAmount.text = amount.ToString();
        }

        public void SetAmount(float amount)
        {
            textAmount.text = amount.ToString();
        }

        public void SetAmount(BigDouble amount)
        {
            textAmount.text = amount.ToLetter();
        }

        private Color GetColor()
        {
            return resourceType switch
            {
                TypeResource.HeartPoint => ExtensionColorText.GetColor("#ECD038"),
                TypeResource.Berry => ExtensionColorText.GetColor("#BF87F1"),
                TypeResource.Ton => ExtensionColorText.GetColor("#2CA3EE"),
                _ => Color.white,
            };
        }

// #if UNITY_EDITOR
//         private void OnValidate()
//         {
//             imageBackground.color = GetColor();
//             imageIcon.sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/_Src/_Sprite/ResourceIcon/{(int)resourceType}.png");
//         }
// #endif
    }
}