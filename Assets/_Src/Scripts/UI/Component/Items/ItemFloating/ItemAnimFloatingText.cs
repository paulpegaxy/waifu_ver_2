// Author: 
// Created Date: 23/07/2024
// Update Time: 23/07

using System;
using BreakInfinity;
using Game.Runtime;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemAnimFloatingText : MonoBehaviour
    {
        [SerializeField] private TypeResource type;
        [SerializeField] private Image imgIcon;
        [SerializeField] private TMP_Text txtText;
        [SerializeField] private Animator anim;

        private void Start()
        {
            Initialize();
        }

        public void SetData(BigDouble amount, int id)
        {
            txtText.text = $"+{amount}";
            imgIcon.sprite = ControllerSprite.Instance.GetResourceIcon(id);
        }

        public void SetDataTest(TypeGirlReact type,BigDouble amount)
        {
            txtText.text = $"{type}: +{amount}";
        }

        private void Initialize()
        {
            imgIcon.sprite = ControllerSprite.Instance.GetResourceIcon(type);
            txtText.text = string.Empty;
            txtText.color = Color.white;
        }

        public void AutoDeSpawn()
        {
            ControllerSpawner.Instance.Return(gameObject);
        }
    }
}