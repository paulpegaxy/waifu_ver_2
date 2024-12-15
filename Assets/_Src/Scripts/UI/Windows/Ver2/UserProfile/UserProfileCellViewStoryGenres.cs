// Author: ad   -
// Created: 14/12/2024  : : 17:12
// DateUpdate: 14/12/2024

using System.Collections.Generic;
using System.Linq;
using Game.Defines;
using Game.Model;
using UnityEngine;

namespace Game.UI
{
    public class UserProfileCellViewStoryGenres : AUserProfileCellView
    {
        [SerializeField] private ItemBarTag itemBarTag;
        
        protected override void OnLoadData(ModelApiChatInfoExtra data)
        {
            List<TypeStoryGenres> listGenres = data.genres.Split(';').Select(x => x.Split('_')[0].ToEnum<TypeStoryGenres>()).ToList();
            itemBarTag.SetData(listGenres);
        }
    }
}