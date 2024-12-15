using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Model;
using Game.Runtime;
using Game.UI;
using Game.UI.Ver2.Swipe.Item;
using Template.Runtime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SwipeCharWindow : UIWindow
{
    [SerializeField] private Image imgBannerCharOld;
    [SerializeField] private Image imgBannerCharNext;
    [SerializeField] private Image imgBannerChar;
    [SerializeField] private ItemManageSwipeChar itemSwipeChar;

    private int _index;
    private int _maxSwipeEleCount;
    private bool _canUndo;
    private int _swipeCountLeft;
    private int _indexPrepare;
    
    private ModelApiEntityConfig _currentEntity;
    private ModelApiEntityConfig _nextEntity;
    


    private ModelApiEntity ApiEntity => FactoryApi.Get<ApiEntity>().Data;

    private void Start()
    {
        _index = FactoryStorage.Get<StorageUserInfo>().Get().charSwipeIndexSelected;
        _indexPrepare = _index + 2;
        // Debug.LogError("_index: " + _index);
    }

    protected override void OnEnabled()
    {
        Clear();
        FactoryApi.Get<ApiChatInfo>().GetInfo().Forget();
        _swipeCountLeft = FactoryApi.Get<ApiChatInfo>().Data.Info.swipe_count;
        _index = FactoryStorage.Get<StorageUserInfo>().Get().charSwipeIndexSelected;
        Refresh();
        ModelApiEntity.OnChanged += OnEntityChanged;
        AItemSwipeCharOverlay.OnUndoCharacter += OnUndoCharacter;
        AItemSwipeCharOverlay.OnRaiseAcceptGirl += ProcessAcceptGirl;
        AItemSwipeCharOverlay.OnRaiseDeclineGirl += ProcessDeclineGirl;
        ControllerDragDrop.OnEndDragWithTarget += OnEndDragWithTarget;
        
        ModelApiChatInfoDetail.OnChanged += OnChatInfoChanged;
    }

    protected override void OnDisabled()
    {
        ModelApiEntity.OnChanged -= OnEntityChanged;
        AItemSwipeCharOverlay.OnUndoCharacter -= OnUndoCharacter;
        AItemSwipeCharOverlay.OnRaiseAcceptGirl -= ProcessAcceptGirl;
        AItemSwipeCharOverlay.OnRaiseDeclineGirl -= ProcessDeclineGirl;
        ControllerDragDrop.OnEndDragWithTarget -= OnEndDragWithTarget;
        
        ModelApiChatInfoDetail.OnChanged -= OnChatInfoChanged;
    }

    private void Clear()
    {
        // imgBannerCharNext.gameObject.SetActive(false);
        imgBannerCharNext.gameObject.SetActive(false);
    }

    private void OnChatInfoChanged(ModelApiChatInfoDetail info)
    {
        _swipeCountLeft = info.swipe_count;
    }

    private void OnEntityChanged(ModelApiEntity entity)
    {
        _maxSwipeEleCount = entity.UnMatchedChars.Count;
        
        // if (entity.UnMatchedChars == null || entity.UnMatchedChars.Count == 0)
        // {
        //     LoadEmpty();
        //     return;
        // }

        // LoadEntityCardInfo(entity.CurrentEntity);
    }

    private async void Refresh()
    {
        // if (ApiEntity.UnMatchedChars == null) return;
        
        _maxSwipeEleCount = ApiEntity.UnMatchedChars.Count;
        _currentEntity = ApiEntity.GetEntityUnMatched(_index);
        
        // ApiEntity.SetCurrentEntity(_index);

        // var currEntity = ApiEntity.CurrentEntity;
        if (_currentEntity == null)
        {
            LoadEmpty();
            return;
        }

        this.ShowProcessing();
        LoadEntityCardInfo(_currentEntity);
        await LoadBannerChar(imgBannerChar, _currentEntity.BgCharKey);
        imgBannerCharOld.sprite = imgBannerChar.sprite;
        // LoadBannerChar(imgBannerCharOld, _currentEntity.BgCharKey);
        PrepareNextBanner();
        PrepareNextPool();
        
        // LoadBannerChar(ref imgBannerCharNext, ApiEntity.NextEntity.BgCharKey);
        
        
        // if (ApiEntity.PrevEntity != null)
        //     LoadBannerChar(ref imgBannerCharPrev, ApiEntity.PrevEntity.BgCharKey);
        
        this.HideProcessing();
    }

    private void LoadEmpty()
    {
        
    }

    private void LoadEntityCardInfo(ModelApiEntityConfig entityConfigData)
    {
        bool outOfSwipe = FactoryApi.Get<ApiChatInfo>().Data.Info.swipe_count <= 0;
        
        TypeSwipeCharItem type = TypeSwipeCharItem.Basic;
        if (outOfSwipe)
            type = TypeSwipeCharItem.OutOfSwipe;
        else
        {
            // if (entityConfigData.IsDeclined)
            //     type = TypeSwipeCharItem.Decline;
            // else 
            if (entityConfigData.match_status == TypeMatchGirlStatus.match)
                type = TypeSwipeCharItem.Accept;
            else
                type = TypeSwipeCharItem.Basic;
        }

        // Debug.LogError("Type " + type + " count " + FactoryApi.Get<ApiGame>().Data.InfoNew.swipe_count);

        itemSwipeChar.SetData(new DataItemSwipeChar()
        {
            charId = entityConfigData.id,
            entityConfig = entityConfigData,
            type = type
        });
    }

    private async UniTask LoadBannerChar(Image banner, string key)
    {
        await banner.LoadSpriteAsync(key);
    }

    private void OnEndDragWithTarget(bool isSwipeRightSide)
    {
        if (isSwipeRightSide)
        {
            if (_swipeCountLeft <= 0)
            {
                ControllerPopup.ShowToastError("Out of swipes");
                return;
            }

            ProcessDeclineGirl();
        }
        else
        {
            ProcessAcceptGirl();
        }
    }

    private async void ProcessAcceptGirl()
    {
        this.ShowProcessing();
        try
        {
            _swipeCountLeft--;
            var result=await FactoryApi.Get<ApiEntity>().PostAcceptGirl(_currentEntity.id);
            this.HideProcessing();
            if (result != null)
            {
                FactoryApi.Get<ApiChatInfo>().GetInfo().Forget();
                var popup = this.ShowPopup<PopupSuccessMatch>(UIId.UIPopupName.PopupSuccessMatch);
                popup.SetData(result);
                MoveAccept(true);
            }
            else
            {
                ControllerPopup.ShowToastError("Accept fail!\nThis girl don't like you! Please try again");
                MoveAccept(false);
            }
        }
        catch (Exception e)
        {
            e.ShowError();
        }
    }

    private void ProcessDeclineGirl()
    {
        try
        {
            _swipeCountLeft--;
            FactoryApi.Get<ApiEntity>().PostDeclineGirl(_currentEntity.id).Forget();
            FactoryApi.Get<ApiChatInfo>().GetInfo().Forget();
            MoveDecline();
        }
        catch (Exception e)
        {
            e.ShowError();
        }
    }

    private void MoveDecline()
    {
        itemSwipeChar.transform.DOScaleY(0, 0);
 
        _index++;
        // Debug.LogError("MoveDecline: " + _index);
        if (_index >= _maxSwipeEleCount)
            _index = 0;
        _currentEntity = ApiEntity.GetEntityUnMatched(_index);
        _canUndo = true;
        ProcessMoveBanner(-1);
    }

    private void MoveAccept(bool isMatchSuccess)
    {
        itemSwipeChar.transform.DOScaleY(0, 0);
        if (isMatchSuccess)
        {
            _maxSwipeEleCount = ApiEntity.UnMatchedChars.Count;
            var entity = ApiEntity.GetEntityUnMatched(_index);
            if (_currentEntity.id == entity.id)
            {
                _index++;
            }

            if (_index >= _maxSwipeEleCount)
                _index = 0;
            _canUndo = false;
        }
        else
        {
            _index++;
            _canUndo = true;
        }

        _currentEntity = ApiEntity.GetEntityUnMatched(_index);
        
        ProcessMoveBanner(1);
    }

    private void MoveUndo()
    {
        _canUndo = false;
        itemSwipeChar.transform.DOScaleY(0, 0);
        _index--;
        if (_index < 0)
            _index = _maxSwipeEleCount - 1;
        _currentEntity = ApiEntity.GetEntityUnMatched(_index);
        ProcessMoveBanner(1, true);
    }

    private void ProcessMoveBanner(float valueX, bool isUndo = false)
    {
        var bannerTransform = imgBannerChar.GetComponent<RectTransform>();
        if (isUndo)
            imgBannerCharOld.gameObject.SetActive(true);
        else
            imgBannerCharNext.gameObject.SetActive(true);
        bannerTransform.DOAnchorPosX(valueX * bannerTransform.sizeDelta.x, 0.35f).OnComplete(() =>
        {
            bannerTransform.anchoredPosition = Vector2.zero;
        });
        bannerTransform.DORotate(new Vector3(0, 0, 45 * -valueX), 0.35f).OnComplete(() =>
        {
            bannerTransform.rotation = Quaternion.identity;
            ProcessAfterMoveBanner(isUndo);
        });
    }

    private async void PrepareNextBanner()
    {
        int nextIndex = _index + 1;
        if (nextIndex >= _maxSwipeEleCount)
            nextIndex = 0;
        _nextEntity = ApiEntity.GetEntityUnMatched(nextIndex);
        if (_nextEntity != null)
        {
            // imgBannerCharNext.sprite = null;
            await LoadBannerChar(imgBannerCharNext, _nextEntity.BgCharKey);
            // Debug.LogError("Done Prepare next banner: " + nextInfo.BgCharKey);
        }
    }
    
    private async void PrepareNextPool()
    {
        ApiEntity.PrepareNextPool(_indexPrepare).Forget();
        _indexPrepare += GameConsts.MAX_LENGTH_NEXT_PREPARE_WAIFU_BANNER;
    }

    private async void ProcessAfterMoveBanner(bool isUndo = false)
    {
        // Debug.LogError("ProcessAfterMoveBanner: " + _index + ", current: " + _currentEntity.id);
        if (!isUndo)
        {
            imgBannerCharOld.sprite = imgBannerChar.sprite;
            if (_nextEntity.id == _currentEntity.id)
            {
                // Debug.LogError("Next entity is current entity");
                imgBannerChar.sprite = imgBannerCharNext.sprite;
            }
            else
            {
                this.ShowProcessing();
                await LoadBannerChar(imgBannerChar, _currentEntity.BgCharKey);
                this.HideProcessing();
            }
        }
        else
        {
            imgBannerChar.sprite = imgBannerCharOld.sprite;
        }

        imgBannerCharNext.gameObject.SetActive(false);
        imgBannerCharOld.gameObject.SetActive(false);

        // if (!isUndo)
        // {
        //     // FactoryApi.Get<ApiChatInfo>().PostMatchGirl();
        //     FactoryApi.Get<ApiChatInfo>().GetInfo().Forget();
        // }

        LoadEntityCardInfo(_currentEntity);
        itemSwipeChar.transform.DOScaleY(1, 0f);
        PrepareNextBanner();
        PrepareNextPool();
        var userInfo = FactoryStorage.Get<StorageUserInfo>();
        userInfo.Get().charSwipeIndexSelected = _index;
        userInfo.Save();

        // LoadBannerChar(ref imgBannerCharNext, ApiEntity.NextEntity.BgCharKey);
        // if (ApiEntity.PrevEntity != null)
        //     LoadBannerChar(ref imgBannerCharPrev, ApiEntity.PrevEntity.BgCharKey);
    }

    private void OnUndoCharacter()
    {
        if (!_canUndo)
        {
            ControllerPopup.ShowToastError("Can't undo");
            return;
        }
        
        MoveUndo();
    }
}
