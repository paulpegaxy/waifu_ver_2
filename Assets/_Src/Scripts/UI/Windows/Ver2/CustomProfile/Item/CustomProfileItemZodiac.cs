using System.Collections;
using System.Collections.Generic;
using Game.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class CustomProfileItemZodiac : MonoBehaviour
    {
        [SerializeField] private Image imgIcon;
        [SerializeField] private TMP_Text txtDes;

        public void SetData(TypeZodiac type)
        {
            txtDes.text = type.ToString();
            imgIcon.sprite = ControllerSprite.Instance.GetZodiacIcon(type);
            imgIcon.SetNativeSize();
        }
    }
}
