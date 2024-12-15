using System;
using BreakInfinity;

namespace Game.Model
{
    [Serializable]
    public class ModelApiBoosterNext
    {
        public ModelApiBoosterNextPointTap point_tap;
        public ModelApiBoosterNextStamina stamina_max;
        public int next_charge_stamina_wait_time;
    }

    [Serializable]
    public class ModelApiBoosterNextPointTap : ModelApiUpgradeCostParse
    {
        public int point_per_tap;
    }

    [Serializable]
    public class ModelApiBoosterNextStamina : ModelApiUpgradeCostParse
    {
        public int max_stamina;
    }
}