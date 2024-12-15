
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Game.Runtime;
using UnityEngine;

public class CheatComponent : MonoBehaviour
{
    [SerializeField] private UIButton btnResetData;
    [SerializeField] private UIButton btnCheatPoint;
    [SerializeField] private UIButton btnCheatPointSecond;
    [SerializeField] private UIButton btnCheatBerry;
    [SerializeField] private UIButton btnCheatZeroStamina;
    [SerializeField] private UIButton btnCheatPartner;

    private void OnEnable()
    {
        btnResetData.onClickEvent.AddListener(OnClickResetData);
        btnCheatPoint.onClickEvent.AddListener(OnClickCheatPoint);
        btnCheatBerry.onClickEvent.AddListener(OnClickCheatBerry);
        btnCheatZeroStamina.onClickEvent.AddListener(OnClickCheatZeroStamina);
        btnCheatPointSecond.onClickEvent.AddListener(OnClickCheatPointSecond);
        btnCheatPartner.onClickEvent.AddListener(OnClickCheatPartner);
    }

    private void OnDisable()
    {
        btnResetData.onClickEvent.RemoveListener(OnClickResetData);
        btnCheatPoint.onClickEvent.RemoveListener(OnClickCheatPoint);
        btnCheatBerry.onClickEvent.RemoveListener(OnClickCheatBerry);
        btnCheatZeroStamina.onClickEvent.RemoveListener(OnClickCheatZeroStamina);
        btnCheatPointSecond.onClickEvent.RemoveListener(OnClickCheatPointSecond);
        btnCheatPartner.onClickEvent.RemoveListener(OnClickCheatPartner);
    }

    private void OnClickResetData()
    {
        ControllerPopup.ShowWarning("Are you sure want to reset data?", "Yes", "No", (popup) =>
        {
            ProcessResetData(popup).Forget();
        });
    }

    private async void OnClickCheatBerry()
    {
        this.ShowProcessing();
        FactoryApi.Get<ApiGame>().CheatBerry();
        this.HideProcessing();
    }

    private async void OnClickCheatZeroStamina()
    {
        this.ShowProcessing();
        await FactoryApi.Get<ApiGame>().CheatZeroStamina();
        this.HideProcessing();
    }

    private async UniTask ProcessResetData(UIPopup popup)
    {
        // await FactoryApi.Get<ApiGame>().ResetData();
        await FactoryApi.Get<ApiUser>().ResetAccount();
        // FactoryApi.CleanUp();
        PlayerPrefs.DeleteAll();
        FactoryStorage.Init();
        // FactoryGameManager.InitAfterSceneLoad();
        TutorialMgr.Instance.Init();
        AnR.ReleaseAll();
        ControllerSpawner.Instance.ReleaseAll();
        ControllerAutomation.Stop();
        // await FactoryApi.Get<ApiGame>().OldCheatResetData();
        // await FactoryApi.Get<ApiUpgrade>().Get();
        // await FactoryApi.Get<ApiUser>().CheatResetAllBotData();
        popup.Hide();



#if !UNITY_EDITOR
            // Application.OpenURL("about:blank");
// this.ShowPopup(UIId.UIPopupName.PopupReloadGame);
            TelegramWebApp.Reload();
               // UnityEngine.Device.Application.Quit();
#else
        UnityEditor.EditorApplication.isPlaying = false;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
#endif


        // TutorialMgr.Instance.Show(TutorialCategory.Main);
    }

    private async void OnClickCheatPoint()
    {
        await FactoryApi.Get<ApiChatInfo>().PostCheat();
        ControllerPopup.ShowToastSuccess("Success add 100 chat point and 10 swipe count");
        // FactoryApi.Get<ApiGame>().CheatPoint();
    }

    private void OnClickCheatPointSecond()
    {
        FactoryApi.Get<ApiGame>().CheatPoint(300);
    }

    private async void OnClickCheatPartner()
    {
        FactoryApi.Get<ApiUser>().Data.AddCheatTagPartner();
        ControllerPopup.ShowToastSuccess("Success add tag");
    }
}
