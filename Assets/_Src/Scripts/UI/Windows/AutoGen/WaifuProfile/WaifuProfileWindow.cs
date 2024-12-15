// Author: ad   -
// Created: 02/12/2024  : : 02:12
// DateUpdate: 02/12/2024

using System;
using System.Collections.Generic;
using System.Linq;
using Doozy.Runtime.UIManager.Components;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Template.Defines;
using UnityEngine;

namespace Game.UI
{
    public class WaifuProfileWindow : UIWindow
    {
        [SerializeField] private WaifuProfileScroller scroller;
        [SerializeField] private UIButton btnBack;
        
        private ModelApiEntityConfig _data;

        protected override void OnEnabled()
        {
            btnBack.onClickEvent.AddListener(OnClickBack);
            base.OnEnabled();
            var data = this.GetEventData<TypeGameEvent, ModelApiEntityConfig>(TypeGameEvent.OpenGallery, true);
            if (data != null)
            {
                LoadData(data);
            }
        }

        private async void LoadData(ModelApiEntityConfig config)
        {
            var apiEntity = FactoryApi.Get<ApiEntity>();
            await apiEntity.Get();
            var entity = apiEntity.Data.GetEntity(config.id);
            _data = entity;
            
            List<ModelWaifuProfileCellView> listData = new List<ModelWaifuProfileCellView>();
            listData.Add(new ModelWaifuProfileCellViewHeader()
            {
                Data = _data
            });

            var dataList = new List<DataItemWaifuProfilePicture>();
            for (var i = 0; i < GameConsts.MAX_WAIFU_PICTURE; i++)
            {
                dataList.Add(new DataItemWaifuProfilePicture()
                {
                    data = _data,
                    index = i
                });
            }

            var groupedItems = new List<List<DataItemWaifuProfilePicture>>();
            for (var i = 0; i < GameConsts.MAX_WAIFU_PICTURE; i += 3)
            {
                var sublist = dataList.Skip(i).Take(3).ToList();
                groupedItems.Add(sublist);
            }
                
            foreach (var group in groupedItems)
            {
                listData.Add(new ModelWaifuProfileCellViewContentPicture()
                {
                    RowData = group
                });
            }
                
            scroller.SetData(listData);
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            btnBack.onClickEvent.RemoveListener(OnClickBack);
        }
        
        private void OnClickBack()
        {
            this.GotoDatingWindow(_data);
        }
    }
}