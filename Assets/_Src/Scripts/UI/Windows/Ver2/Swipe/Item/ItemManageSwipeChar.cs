using System;
using System.Linq;
using _Src.Scripts.UI.Popups;
using Game.Model;
using Game.UI.Ver2.Swipe.Item;
using UnityEngine;

namespace Game.UI
{
    public class ItemManageSwipeChar : MonoBehaviour
    {
        [SerializeField] private EleSwipeCharCardOverlay[] arrCardOverlay;

        private DataItemSwipeChar _data;
        private TypeSwipeCharItem _type;

        public void SetData(DataItemSwipeChar data)
        {
            _data = data;
            _type = data.type;
            LoadContent(_type);
        }

        private void LoadContent(TypeSwipeCharItem type)
        {
            for (var i = 0; i < arrCardOverlay.Length; i++)
            {
                var ele = arrCardOverlay[i];
                if (ele.type == type)
                {
                    ele.refData.SetData(_data);
                    ele.refData.gameObject.SetActive(true);
                }
                else
                {
                    ele.refData.gameObject.SetActive(false);
                }
            }
        }

        private void OnEnable()
        {
            AItemSwipeCharOverlay.OnSeeRateInfo += OnSeeRateInfo;
        }

        private void OnDisable()
        {
            AItemSwipeCharOverlay.OnSeeRateInfo -= OnSeeRateInfo;
        }
        
        private void OnSeeRateInfo(bool isShow)
        {
            if (isShow)
            {
                LoadContent(TypeSwipeCharItem.RateInfo);
            }
            else
            {
                LoadContent(_type);
            }
        }
    }

    public enum TypeSwipeCharItem
    {
        Basic,
        RateInfo,
        Decline,
        OutOfSwipe,
        Accept
    }
    
    [Serializable]
    public class EleSwipeCharCardOverlay : BaseEleUIType<TypeSwipeCharItem,AItemSwipeCharOverlay>
    {
        
    }

    [Serializable]
    public class DataItemSwipeChar
    {
        public int charId;
        public ModelApiEntityConfig entityConfig;
        public TypeSwipeCharItem type;
    }
}