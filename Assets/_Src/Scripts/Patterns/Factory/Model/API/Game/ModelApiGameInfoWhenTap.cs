// Author: ad   -
// Created: 13/10/2024  : : 23:10
// DateUpdate: 13/10/2024

namespace Game.Model
{
    public class ModelApiGameInfoCachePost
    {
        public string point;
        public string stamina;
        public string tutorial_step;
        public int current_level_girl;
        public int stamina_max;
        
        public ModelApiGameInfoNextData next;

        
        public int stamina_per_second;
        public string berry;
        public string point_per_tap;
        public string point_all_time;
        public string berry_all_time;
        public float point_per_second;

        public void ConvertSave(ref ModelApiGameInfo info)
        {
            info.point = point;
            info.stamina = stamina;
            info.tutorial_step= tutorial_step;
            info.current_level_girl = current_level_girl;
            info.stamina_max = stamina_max;

            info.point_per_second = point_per_second;
            
            info.next = next;
            
            info.stamina_per_second = stamina_per_second;
            info.berry = berry;
            info.point_per_tap = point_per_tap;
            info.point_all_time = point_all_time;
            info.berry_all_time = berry_all_time;
            
        }
    }
}