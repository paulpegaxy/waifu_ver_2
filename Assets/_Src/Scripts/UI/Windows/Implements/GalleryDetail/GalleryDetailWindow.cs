
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Extensions;
using Game.Runtime;
using Template.Defines;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GalleryDetailWindow : UIWindow
    {
        [SerializeField] private RawImage imgBg;
        [SerializeField] private Transform anchorPos;
        [SerializeField] private GalleryDetailPanelWaifu panelNormal;
        [SerializeField] private GalleryDetailPanelWaifuPremium panelPremium;

        private DataItemGallery _cacheData;

        private void Awake()
        {
            panelPremium.gameObject.SetActive(false);
            panelNormal.gameObject.SetActive(false);
        }

        protected override void OnEnabled()
        {
            var data = this.GetEventData<TypeGameEvent, DataItemGallery>(TypeGameEvent.GalleryDetail);
            if (data == null)
                return;

            _cacheData = data;
            LoadData(data);

        }

        protected override void OnDisabled()
        {
            if (anchorPos.childCount > 0)
            {
                for (int i=anchorPos.childCount-1; i>=0; i--)
                {
                    DestroyImmediate(anchorPos.GetChild(i).gameObject);
                }
            }
            // this.RemoveEvent(TypeGameEvent.ChangeGirl, OnChangeGirl);
        }

        private void OnChangeGirl(object data)
        {
            LoadData(_cacheData);
        }

        private async void LoadData(DataItemGallery data)
        {
            this.ShowProcessing();
            var go = AnR.Get<GameObject>($"{data.girlId}_spine");
            if (go == null)
            {
                await AnR.LoadAddressableByLabels<Texture>(new List<string>() { data.girlId.ToString() });
                await AnR.LoadAddressableByLabels<GameObject>(new List<string>() { data.girlId.ToString() });
            }

            await SpawnBackground(data.girlId);
            
            bool isPremium = data is DataItemGalleryWaifuPremium;
            if (isPremium)
            {
                panelPremium.Show(data);
                panelNormal.gameObject.SetActive(false);
            }
            else
            {
                panelNormal.Show(data);
                panelPremium.gameObject.SetActive(false);
            }
            this.HideProcessing();
        }
        
        private async UniTask SpawnBackground(int girlId)
        {
            var bg = await ControllerSpawner.Instance.SpawnBackground(DBM.Config.backgroundConfig.GetBackgroundCharNormal(girlId, 0));
            imgBg.texture = bg;
        }

        // [SerializeField] private Transform posContainIndicator;
        // [SerializeField] private RawImage imgBg;
        // [SerializeField] private Transform posContainChar;
        // [SerializeField] private GameObject objRank;
        // [SerializeField] private Image imgRank;
        // [SerializeField] private TMP_Text txtRank;
        // [SerializeField] private GameObject holderBtn;
        // [SerializeField] private UIButton btnNext, btnPrevious;
        //
        // [BoxGroup("SELECT BAR")]
        // [SerializeField] private GameObject objComplete;
        // [BoxGroup("SELECT BAR")]
        // [SerializeField] private UIButton btnSelect;
        // [BoxGroup("SELECT BAR")]
        // [SerializeField] private TMP_Text txtBtnComplete;
        //
        // private GirlEntity _currentGirl;
        // private int _entityId;
        // private int _levelSelected;
        // private int _maxLevelGirl;
        //
        // [SerializeField, Unity.Collections.ReadOnly] private List<GalleryItemIndicator> _listIndicatorActive;

        // private void OnEnable()
        // {
        //     btnNext.onClickEvent.AddListener(OnClickNext);
        //     btnPrevious.onClickEvent.AddListener(OnClickPrevious);
        //     var data = this.GetEventData<TypeGameEvent, DataItemGalleryWaifu>(TypeGameEvent.GalleryDetail);
        //     if (data == null)
        //         return;
        //
        //     _entityId = data.girlId;
        //     Clear(true);
        //     LoadData(data);
        // }
        //
        // private void OnDisable()
        // {
        //     btnNext.onClickEvent.RemoveListener(OnClickNext);
        //     btnPrevious.onClickEvent.RemoveListener(OnClickPrevious);
        //     if (_currentGirl == null)
        //         return;
        //
        //     _currentGirl.DestroyObject();
        //     Destroy(_currentGirl.gameObject);
        // }
        //
        // private void Clear(bool isClear)
        // {
        //     objRank.SetActive(!isClear);
        //     holderBtn.SetActive(!isClear);
        // }
        //
        // private async void LoadData(DataItemGalleryWaifu data)
        // {
        //     this.ShowProcessing();
        //     var go = AnR.Get<GameObject>($"{_entityId}_spine");
        //     if (go == null)
        //     {
        //         await AnR.LoadAddressableByLabels<Texture>(new List<string>() { _entityId.ToString() });
        //         await AnR.LoadAddressableByLabels<GameObject>(new List<string>() { _entityId.ToString() });
        //     }
        //     LoadStatusData(data.isDone);
        //     await SpawnBackground();
        //     SpawnGirl(() =>
        //     {
        //         // imgRank.sprite = ControllerSprite.Instance.GetLeagueGirlIcon(data.charRankType);
        //         imgRank.LoadSpriteAutoParseAsync($"league_{(int)data.charRankType}");
        //         txtRank.color = data.colorRank;
        //         objRank.SetActive(true);
        //         _levelSelected = 0;
        //         var gameInfo = FactoryApi.Get<ApiGame>().Data.Info;
        //         if (data.isDone)
        //             _maxLevelGirl = GameConsts.MAX_LEVEL_PER_CHAR - 1;
        //         else
        //             _maxLevelGirl = Math.Clamp(gameInfo.current_level_girl % GameConsts.MAX_LEVEL_PER_CHAR, 0, GameConsts.MAX_LEVEL_PER_CHAR);
        //
        //         // UnityEngine.Debug.LogError("IS Done : "+data.isDone+ ", Is Max Level" + _maxLevelGirl + ", currLv: " + gameInfo.current_level_girl);
        //
        //         if (_maxLevelGirl > 0)
        //         {
        //             holderBtn.SetActive(true);
        //             posContainIndicator.gameObject.SetActive(true);
        //             ProcessIndicator();
        //             _listIndicatorActive[0].SetSelected(true);
        //         }
        //         else
        //             posContainIndicator.gameObject.SetActive(false);
        //         this.HideProcessing();
        //     });
        // }
        //
        // private void ProcessIndicator()
        // {
        //     try
        //     {
        //         List<bool> listIndicator = new();
        //         for (int i = 0; i < GameConsts.MAX_LEVEL_PER_CHAR; i++)
        //         {
        //             listIndicator.Add(i <= _maxLevelGirl);
        //         }
        //
        //         _listIndicatorActive = new List<GalleryItemIndicator>();
        //         posContainIndicator.FillData<bool, GalleryItemIndicator>(listIndicator, (dataIndicator, view, index) =>
        //         {
        //             view.LoadData(dataIndicator);
        //             if (dataIndicator)
        //                 _listIndicatorActive.Add(view);
        //         });
        //
        //     }
        //     catch (Exception e)
        //     {
        //         e.ShowError();
        //     }
        // }
        //
        // private async UniTask SpawnBackground()
        // {
        //     var bg = await ControllerSpawner.Instance.SpawnBackground(_entityId);
        //     imgBg.texture = bg;
        // }
        //
        // private void SpawnGirl(Action callBack)
        // {
        //     _currentGirl = ControllerSpawner.Instance.SpawnGirl(_entityId, posContainChar).GetComponent<GirlEntity>();
        //     _currentGirl.InitToShowReward(_entityId, callBack);
        //
        // }
        //
        // private void LoadStatusData(bool isCompleted)
        // {
        //     objComplete.SetActive(true);
        //     txtBtnComplete.text = !isCompleted
        //         ? Localization.Get(TextId.Gallery_Selected)
        //         : Localization.Get(TextId.Gallery_LevelCompleted);
        //     
        //     //temp
        //     btnSelect.gameObject.SetActive(false);
        // }
        //
        // private void OnClickNext()
        // {
        //     _listIndicatorActive[_levelSelected].SetSelected(false);
        //     _levelSelected++;
        //     if (_levelSelected > _maxLevelGirl)
        //         _levelSelected = 0;
        //     _currentGirl.OnChangeVisual(_levelSelected);
        //     _listIndicatorActive[_levelSelected].SetSelected(true);
        // }
        //
        // private void OnClickPrevious()
        // {
        //     _listIndicatorActive[_levelSelected].SetSelected(false);
        //     _levelSelected--;
        //     if (_levelSelected < 0)
        //         _levelSelected = _maxLevelGirl;
        //     _currentGirl.OnChangeVisual(_levelSelected);
        //     _listIndicatorActive[_levelSelected].SetSelected(true);
        // }
    }
}

