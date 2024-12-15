// Author: ad   -
// Created: 14/11/2024  : : 00:11
// DateUpdate: 14/11/2024

using Game.Model;
using Game.Runtime;
using UnityEngine;

namespace Game.UI
{
    public class PopupJackpotMyHistory : MonoBehaviour
    {
        [SerializeField] private Transform posContain;
        
        private async void OnEnable()
        {
            this.ShowProcessing();
            posContain.gameObject.SetActive(false);
            var apiEvent = FactoryApi.Get<ApiEvent>();
            var listData = await apiEvent.GetMyJackpotHistoryList();
            posContain.gameObject.SetActive(true);
            
            posContain.FillData<ModelApiEventJackpotMyHistory, ItemJackpotMyHistory>(listData, (data, view, index) =>
            {
                view.LoadData(data);
            });
            posContain.gameObject.SetActive(true);
            this.HideProcessing();
        }
    }
}