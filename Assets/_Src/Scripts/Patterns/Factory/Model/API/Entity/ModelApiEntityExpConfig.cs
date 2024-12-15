// Author: ad   -
// Created: 14/12/2024  : : 21:12
// DateUpdate: 14/12/2024

using System;
using UnityEngine.Serialization;

namespace Game.Model
{
    [Serializable]
    public class ModelApiEntityExpConfig
    {
        public int Level;
        public int EXP;
    }

    [Serializable]
    public class ModelApiEntityExpDisplayData
    {
        [FormerlySerializedAs("running_exp")] [FormerlySerializedAs("start_exp")] public int running_exp_at_lv;
        [FormerlySerializedAs("end_exp")] public int end_exp_at_lv;
        public int total_exp_at_level;
        
        public float GetSliderValue()
        {
            return (float) running_exp_at_lv / end_exp_at_lv;
        }
    }
}