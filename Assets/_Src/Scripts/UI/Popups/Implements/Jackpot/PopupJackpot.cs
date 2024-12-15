using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.Signals;
using Game.Core;
using Game.Runtime;
using Game.Model;

namespace Game.UI
{
    public class PopupJackpot : MonoBehaviour
    {
        [SerializeField] private JackpotHeader header;
        [SerializeField] private UIToggle toggleDontShow;
        [SerializeField] private UIButton buttonGo;

        private void Start()
        {
            buttonGo.onClickEvent.AddListener(OnGo);
            toggleDontShow.onClickEvent.AddListener(OnDontShow);
        }

        private void OnDestroy()
        {
            buttonGo.onClickEvent.RemoveListener(OnGo);
            toggleDontShow.onClickEvent.RemoveListener(OnDontShow);
        }

        private void OnGo()
        {
            Signal.Send(StreamId.UI.Jackpot);
        }

        private void OnDontShow()
        {
            var storageSetting = FactoryStorage.Get<StorageSettings>();
            var model = storageSetting.Get();

            model.dontShowJackpot = toggleDontShow.isOn;
            storageSetting.Save();
        }

        public void SetData(ModelApiEventJackpot data, bool isShowActionButton = true)
        {
            header.SetData(data);
            toggleDontShow.gameObject.SetActive(isShowActionButton);
            buttonGo.gameObject.SetActive(isShowActionButton);
        }
    }
}