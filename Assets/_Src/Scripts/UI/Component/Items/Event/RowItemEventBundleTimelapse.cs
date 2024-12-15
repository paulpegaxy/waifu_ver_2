// Author: ad   -
// Created: 15/09/2024  : : 12:09
// DateUpdate: 15/09/2024

using System.Collections.Generic;
using Game.Model;
using UnityEngine;

namespace Game.UI
{
    public class RowItemEventBundleTimelapse : MonoBehaviour
    {
        [SerializeField] private Transform posContain;

        public void LoadData(List<ModelApiShopData> listShopData)
        {
            posContain.FillData<ModelApiShopData, ItemEventBundleTimelapse>(listShopData, (data, view, index) =>
            {
                view.SetData(data);
            });
        }
    }
}