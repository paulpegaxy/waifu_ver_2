using System;
using System.Collections.Generic;

namespace Game.Model
{
    [Serializable]
    public class ModelApiBoosterCurrent
    {
        public int current_level_tap;
        public int current_level_stamina_max;
        public int charge_stamina;
        public int charge_stamina_max;
        public int stamina_max;
        public int point_per_tap;
        public List<string> background = new();
        public List<string> tap_effect = new(); 

        // public List<string> myTapEffect = new();

        public bool IsHaveBackground(string bgId)
        {
            return background.Exists(x => x.Equals(bgId));
        }

        public bool IsHaveTapEffect(string tapEffectId)
        {
            return tap_effect.Exists(x => x.Equals(tapEffectId));
        }
    }
}