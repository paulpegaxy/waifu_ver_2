using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class SettingPanelConfig : MonoBehaviour
    {
        [SerializeField] private TMP_Text txtTelegramID;
        [SerializeField] private TMP_Text txtUserName;
        [SerializeField] private UIButton btnCopy;

        private string _telegramID;

        private bool _isInitilized;
    
        private void OnEnable()
        {
            btnCopy.onClickEvent.AddListener(OnCopy);

            if (!_isInitilized)
            {
                _isInitilized = true;
                return;
            }
            
            var apiUser = FactoryApi.Get<ApiUser>().Data.User;
            _telegramID = apiUser.telegram_id;
            txtTelegramID.text = Localization.Get(TextId.Common_TelegramId);
            txtTelegramID.text += ": " + apiUser.telegram_id;
            txtUserName.text = FactoryApi.Get<ApiUser>().Data.User.name;
            txtUserName.text += " - User ID: " + apiUser.user_id;
        }
    
        private void OnDisable()
        {
            btnCopy.onClickEvent.RemoveListener(OnCopy);
        }
        
        private void OnCopy()
        {
            TelegramWebApp.CopyToClipboard(_telegramID);
            ControllerPopup.ShowToast("Copied telegram ID:\n" + _telegramID);
        }
    }
}