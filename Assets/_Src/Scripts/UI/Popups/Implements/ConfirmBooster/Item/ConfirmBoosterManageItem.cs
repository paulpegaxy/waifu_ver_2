using System;
using System.Linq;
using _Src.Scripts.UI.Popups;
using Game.Runtime;
using Sirenix.OdinInspector;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ConfirmBoosterManageItem : MonoBehaviour
    {
        [SerializeField, ReadOnly] private TypeBooster type;
        [SerializeField] private ConfirmBoosterItemInfo cardInfo;
        [SerializeField] private EleConfirmBoosterCardOverlay[] arrCardOverlay;

        public void LoadData(DataConfirmBoosterItem data)
        {
            type = data.type;
            cardInfo.SetData(data);
            foreach (var overlay in arrCardOverlay)
            {
                overlay.refData.SetData(data);
                overlay.refData.gameObject.SetActive(false);
            }
            
            arrCardOverlay.ToList().Find(x=>x.type==data.type).refData.ShowCard();
        }
    }
    
    [Serializable]
    public class EleConfirmBoosterCardOverlay : BaseEleUIType<TypeBooster, AConfirmBoosterItemOverlay>
    {
        
    }

    [Serializable]
    public class DataConfirmBoosterItem
    {
        public TypeBooster type;
        public int level;
        public int value;
    }
}