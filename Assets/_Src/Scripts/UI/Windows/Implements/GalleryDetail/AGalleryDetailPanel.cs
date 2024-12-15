// Author: ad   -
// Created: 16/10/2024  : : 22:10
// DateUpdate: 16/10/2024

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Sirenix.OdinInspector;
using Slime.UI;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.UI
{
    public abstract class AGalleryDetailPanel :  BasePanel
    {
        [SerializeField] protected Transform posContainIndicator;
        [SerializeField] protected Transform posContainChar;
        [SerializeField] protected GameObject holderBtn;
        [SerializeField] protected UIButton btnNext, btnPrevious;
        [SerializeField] protected UIButton btnSelect;
        [SerializeField] protected GameObject objSelected;
        [SerializeField] protected TMP_Text txtBtnSelected;
        
        private GirlEntity _currentGirl;
        private int _entityId;
        
        protected int LevelSelected;
        protected int MaxLevelGirl;
        
        [SerializeField, Unity.Collections.ReadOnly] 
        protected List<GalleryItemIndicator> listIndicatorActive;

        protected DataItemGallery Data;
        private bool _isActionChangeGirl;
        
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Show(DataItemGallery data)
        {
            ClearChar();
            gameObject.SetActive(true);
            _isActionChangeGirl = false;
            _entityId = data.girlId;
            Data = data;
            Clear(true);
            Fetch();
        }

        private void OnEnable()
        {
            btnNext.onClickEvent.AddListener(OnClickNext);
            btnPrevious.onClickEvent.AddListener(OnClickPrevious);
            btnSelect.onClickEvent.AddListener(OnClickSelect);
        }

        private void OnDisable()
        {
            btnNext.onClickEvent.RemoveListener(OnClickNext);
            btnPrevious.onClickEvent.RemoveListener(OnClickPrevious);
            btnSelect.onClickEvent.RemoveListener(OnClickSelect);
            ClearChar();
            // if (_isActionChangeGirl)
            // {
            //     this.PostEvent(TypeGameEvent.ChangeGirl,Data.girlId);
            // }
        }


        private void ClearChar()
        {
            if (_currentGirl == null)
                return;

            _currentGirl.DestroyObject();
            Destroy(_currentGirl.gameObject);
            _currentGirl = null;
        }

        protected void SpawnGirl(Action callBack)
        {
            _currentGirl = ControllerSpawner.Instance.SpawnGirl(_entityId, posContainChar).GetComponent<GirlEntity>();
            _currentGirl.InitToShowReward(_entityId, callBack);
        }
        
        protected void ProcessIndicator()
        {
            try
            {
                List<bool> listIndicator = new();
                for (int i = 0; i < GameConsts.MAX_LEVEL_PER_CHAR; i++)
                {
                    listIndicator.Add(i <= MaxLevelGirl);
                }

                listIndicatorActive = new List<GalleryItemIndicator>();
                posContainIndicator.FillData<bool, GalleryItemIndicator>(listIndicator, (dataIndicator, view, index) =>
                {
                    view.LoadData(dataIndicator);
                    if (dataIndicator)
                        listIndicatorActive.Add(view);
                });

            }
            catch (Exception e)
            {
                e.ShowError();
            }
        }

        protected abstract void Clear(bool isClear);
        protected abstract void OnSelectCharacter();

        private void OnClickSelect()
        {
            OnSelectCharacter();
            objSelected.SetActive(true);
            btnSelect.gameObject.SetActive(false);
            // _isActionChangeGirl = true;
            this.PostEvent(TypeGameEvent.ChangeGirl,Data.girlId);
        }
        
        private void OnClickNext()
        {
            listIndicatorActive[LevelSelected].SetSelected(false);
            LevelSelected++;
            if (LevelSelected > MaxLevelGirl)
                LevelSelected = 0;
            _currentGirl.OnChangeVisual(LevelSelected);
            listIndicatorActive[LevelSelected].SetSelected(true);
        }

        private void OnClickPrevious()
        {
            listIndicatorActive[LevelSelected].SetSelected(false);
            LevelSelected--;
            if (LevelSelected < 0)
                LevelSelected = MaxLevelGirl;
            _currentGirl.OnChangeVisual(LevelSelected);
            listIndicatorActive[LevelSelected].SetSelected(true);
        }
    }
}