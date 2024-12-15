// Author: ad   -
// Created: 01/12/2024  : : 22:12
// DateUpdate: 01/12/2024

using System;
using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ListCharCellView : ESCellView<ModelApiEntityConfig>
    {
        [SerializeField] private UIButton btnCell;
        [SerializeField] private ItemWaifuAvatar itemWaifuAvatar;
        [SerializeField] private TMP_Text txtCharName;
        
        private ModelApiEntityConfig _data;
        
        public override void SetData(ModelApiEntityConfig data)
        {
            _data = data;
            txtCharName.text = data.name;
            itemWaifuAvatar.SetAvatar(data);
        }

        private void OnEnable()
        {
            btnCell.onClickEvent.AddListener(OnClickCell);
        }
        
        private void OnDisable()
        {
            btnCell.onClickEvent.RemoveListener(OnClickCell);
        }

        private void OnClickCell()
        {
            this.GotoDatingWindow(_data);
        }
    }
}