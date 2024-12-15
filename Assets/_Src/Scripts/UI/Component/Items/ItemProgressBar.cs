// Author: ad   -
// Created: 22/09/2024  : : 19:09
// DateUpdate: 22/09/2024

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public abstract class ItemProgressBar<T> : MonoBehaviour where T : DataItemProgress
    {
        [SerializeField] private Image sliderProgress;
        [SerializeField] private Transform posContainItem;
        
        public void LoadData(List<T> listData,int progress)
        {
            if (progress <= 0)
            {
                sliderProgress.fillAmount = 0;
            }
            else
            {
                var progressStep = (float)(progress - 1) / (listData.Count - 1);
                sliderProgress.fillAmount = progressStep;
            }

            posContainItem.FillData<T, ItemProgressElement<T>>(listData,
                (dataItem, view, index) =>
                {
                    view.SetData(dataItem);
                });
        }
        
        
    }

    [Serializable]
    public abstract class ItemProgressElement<T> : MonoBehaviour where T : DataItemProgress
    {
        public void SetData(T data)
        {
            OnSetData(data);
        }

        protected abstract void OnSetData(T data);
    }

    [Serializable]
    public class DataItemProgress
    {
        
    }
}