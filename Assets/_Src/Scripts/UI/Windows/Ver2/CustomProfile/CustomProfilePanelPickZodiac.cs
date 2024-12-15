using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Game.Defines;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class CustomProfilePanelPickZodiac : ACustomProfilePanel
    {
        [SerializeField] private UIToggleGroup tggZodiac;
        [SerializeField] private Transform posContain;

        private string _zodiacName;

        protected override async UniTask OnFetchData()
        {
        }

        protected override void OnLoadData()
        {
            tggZodiac.OnToggleTriggeredCallback.AddListener(OnToggle);
            LoadData();
            if (Data.extra_data?.zodiac.Length > 0)
            {
                var split = Data.extra_data.zodiac.Split('_');
                var index = int.Parse(split[^1]);
                if (index != tggZodiac.lastToggleOnIndex)
                {
                    UnityEngine.Debug.Log("index: " + index + ", lastToggleOnIndex: " + tggZodiac.lastToggleOnIndex +
                                          ", count: " + tggZodiac.transform.childCount);
                    tggZodiac.transform.GetChild(index).GetComponent<UIToggle>().isOn = true;
                }
            }
        }

        protected override void OnSaveData()
        {
            var extraInfo = Data.extra_data;
            string check=_zodiacName + "_" + tggZodiac.lastToggleOnIndex;
            IsModifiedProfile = extraInfo.zodiac.Equals(check) == false;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            tggZodiac.OnToggleTriggeredCallback.RemoveListener(OnToggle);
        }

        private void LoadData()
        {
            var arrZodiac = Enum.GetNames(typeof(TypeZodiac));
            List<TypeZodiac> list = new List<TypeZodiac>();
            for (var i = 0; i < arrZodiac.Length; i++)
            {
                list.Add((TypeZodiac) i);
            }

            posContain.FillData<TypeZodiac, CustomProfileItemZodiac>(list, (data, view, index) =>
            {
                view.SetData(data);
                view.GetComponent<UIToggle>().isOn = false;
            });
        }

        private void OnToggle(UIToggle toggle)
        {
            var lastIndex = tggZodiac.lastToggleOnIndex;
            _zodiacName = toggle.GetComponentInChildren<TMP_Text>().text.ToLower();
        }

        protected override async UniTask OnProcessAction()
        {
            KeyProfile = _zodiacName + "_" + tggZodiac.lastToggleOnIndex;
        }
    }
}