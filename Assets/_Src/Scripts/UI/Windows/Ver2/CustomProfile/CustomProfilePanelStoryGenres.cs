using System;
using System.Collections.Generic;
using System.Linq;
using _Src.Scripts.Data.DBM.Configs;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Game.Defines;
using Game.Runtime;
using UnityEngine;

namespace Game.UI
{
    public class CustomProfilePanelStoryGenres : ACustomProfilePanel
    {
        [SerializeField] private Transform posContain;
        [SerializeField] private UIToggleGroup tggStoryGenres;

        private List<string> _listChosen;

        // private List<CustomProfileItemStoryGenres> _listItemChosen;

        protected override async UniTask OnFetchData()
        {
           
        }

        private void Start()
        {
            var arrStoryGenres = Enum.GetNames(typeof(TypeStoryGenres));
            List<TypeStoryGenres> list = new List<TypeStoryGenres>();
            for (var i = 0; i < arrStoryGenres.Length; i++)
            {
                list.Add((TypeStoryGenres) i);
            }

            _listChosen = new List<string>();
            Dictionary<string,int> dict = new Dictionary<string, int>();
            if (Data.extra_data?.genres.Length > 0)
            {
                List<string> listGenres = Data.extra_data.genres.Split(';').ToList();
                foreach (var item in listGenres)
                {
                    var arr = item.Split('_');
                    if (arr.Length > 1)
                        dict.Add(arr[0], int.Parse(arr[^1]));
                }
            }

            bool status = false;
            posContain.FillData<TypeStoryGenres, CustomProfileItemStoryGenres>(list, (data, view, index) =>
            {
                if (dict.ContainsKey(data.ToString()))
                    status = dict[data.ToString()] == index;
                else
                    status = false;
                
                view.SetData(data, status);
                if (status)
                {
                    _listChosen.Add(view.ItemName);
                }
            });

            // _listItemChosen = new List<CustomProfileItemStoryGenres>();
            tggStoryGenres.OnToggleTriggeredCallback.AddListener(OnToggle);
        }

        private void OnDestroy()
        {
            tggStoryGenres.OnToggleTriggeredCallback.RemoveListener(OnToggle);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _listChosen = new List<string>();
        }

        protected override void OnDisable()
        {
            for (int i = 0; i < tggStoryGenres.transform.childCount; i++)
            {
                var ele= tggStoryGenres.transform.GetChild(i).GetComponent<CustomProfileItemStoryGenres>();
                if (ele.Toggle.isOn)
                    ele.Toggle.isOn = false;
            }
            _listChosen = new List<string>();
            base.OnDisable();
        }

        private void OnToggle(UIToggle toggle)
        {
            var lastIndex = tggStoryGenres.lastToggleOnIndex;
            
            var item = toggle.GetComponent<CustomProfileItemStoryGenres>();
            if (toggle.isOn)
            {
                if (_listChosen.Count >= GameConsts.MAX_PICK_STORY_GENRES)
                {
                    ControllerPopup.ShowToastError("You can only choose up to 3 genres");
                    toggle.isOn = false;
                    return;
                }
                _listChosen.Add(item.ItemName);
            }
            else
            {
                if (_listChosen.Contains(item.ItemName))
                {
                    _listChosen.Remove(item.ItemName);
                }
            }
            // UnityEngine.Debug.LogError($"trigger name {toggle.gameObject}: status: {toggle.isOn}");
        }

        protected override void OnLoadData()
        {
            
        }

        protected override void OnSaveData()
        {
            IsModifiedProfile = true;
        }

        protected override async UniTask OnProcessAction()
        {
            KeyProfile = string.Join(";", _listChosen);
            // UnityEngine.Debug.Log(KeyProfile);
        }

        protected override void OnClickNext()
        {
            if (_listChosen.Count < GameConsts.MAX_PICK_STORY_GENRES)
            {
                ControllerPopup.ShowToastError("You must choose at least 3 genres");
                return;
            }
            base.OnClickNext();
        }
    }
}