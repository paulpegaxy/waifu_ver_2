using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class CustomProfilePanelHobby : ACustomProfilePanel
    {
        [SerializeField] private TMP_Text txtTitle;
        [SerializeField] private UIToggleGroup tgHobby;

        private int _hobbyChooseIndex;

        protected override async UniTask OnFetchData()
        {
        }
        
        protected override void OnLoadData()
        {
            tgHobby.OnToggleTriggeredCallback.AddListener(OnToggle);
            if (Data.extra_data?.interested_in.Length > 0)
            {
                var index = int.Parse(Data.extra_data.interested_in);
                UnityEngine.Debug.Log("index" + index);
                if (index != _hobbyChooseIndex)
                {
                    tgHobby.transform.GetChild(index).GetComponent<UIToggle>().isOn = true;
                }
            }
        }

        protected override void OnSaveData()
        {
            var extraInfo = Data.extra_data;
            IsModifiedProfile = extraInfo.interested_in.Equals(_hobbyChooseIndex.ToString()) == false;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            tgHobby.OnToggleTriggeredCallback.RemoveListener(OnToggle);
        }
        
        private void OnToggle(UIToggle toggle)
        {
            var lastIndex = tgHobby.lastToggleOnIndex;
            // txtTitle.text = toggle.GetComponentInChildren<TMP_Text>().text;
            _hobbyChooseIndex = lastIndex;
        }

        protected override async UniTask OnProcessAction()
        {
            KeyProfile = _hobbyChooseIndex.ToString();
        }
    }
}