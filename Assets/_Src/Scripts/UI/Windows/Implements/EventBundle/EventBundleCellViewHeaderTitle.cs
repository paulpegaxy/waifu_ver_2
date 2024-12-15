// Author: ad   -
// Created: 29/10/2024  : : 23:10
// DateUpdate: 29/10/2024

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class EventBundleCellViewHeaderTitle : ESCellView<AModelEventBundleCellView>
    {
        [SerializeField] private TMP_Text txtTitle;
        [SerializeField] private Image imgHeader;
        
        public override void SetData(AModelEventBundleCellView data)
        {
            if (data is ModelEventBundleCellViewHeaderTitle modelData)
            {
                // txtTitle.text = modelData.EventId.SnakeToPascal();

                txtTitle.text = modelData.EventId.Replace("_", " ").ToTitleCase();
                imgHeader.LoadSpriteAutoParseAsync("header_" + modelData.EventId);
            }
        }
    }
}