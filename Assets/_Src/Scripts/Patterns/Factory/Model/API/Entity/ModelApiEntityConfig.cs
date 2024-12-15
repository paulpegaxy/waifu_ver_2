// Author: ad   -
// Created: 30/11/2024  : : 16:11
// DateUpdate: 30/11/2024

using System;
using System.Collections.Generic;
using System.Linq;
using Game.Defines;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Model
{
    [Serializable]
    public class ModelApiEntityConfig
    {
        public int id;
        public string name;
        public List<string> genres;
        public string bio;
        public float match_rate;
        public int year_old;
        public TypeMatchGirlStatus match_status;
        
        public int level;
        
        public int exp;
        public int expRequire;
        public int pictures_number;
        
        // public bool IsDeclined;

        [JsonIgnore] public int ClientCharId => 40000 + id;

        [JsonIgnore] public string BgCharKey => ClientCharId + "_bg";

        [JsonIgnore] public string AvaCharKey => "ava_" + ClientCharId;

        public string GetPercentMatchRateString()
        {
            return (match_rate * 100) + "%";
        }

        public List<TypeStoryGenres> GetGenresList()
        {
            return genres.Select(x => (TypeStoryGenres) Enum.Parse(typeof(TypeStoryGenres), x)).ToList();   
            // return Genres.Split(';').Select(x => (TypeStoryGenres) Enum.Parse(typeof(TypeStoryGenres), x)).ToList();
        }
        
        public string GetMediaPictureKey(int level)
        {
            return $"{ClientCharId}_picturelv_{level}";
        }

        public Color GetAvatarHolderColor()
        {
            int max = 3;
            int index = level / (GameConsts.MAX_WAIFU_PICTURE / max);
            index = Mathf.Clamp(index, 0, 3);
            return DBM.Config.visualConfig.GetWaifuAvaHolderColor(index);
        }
    }

    public enum TypeMatchGirlStatus
    {
        none,
        match,
        decline
    }
}