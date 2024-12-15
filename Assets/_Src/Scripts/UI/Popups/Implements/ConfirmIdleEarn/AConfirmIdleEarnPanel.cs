using System;
using _Src.Scripts.UI.Popups;
using Slime.UI;

namespace Game.UI
{
    public abstract class AConfirmIdleEarnPanel : BaseCardItem<DataIdleEarnUpgradeItem>
    {
        
    }

    [Serializable]
    public class ConfirmIdleEarnPanel : BaseEleUIType<TypeConfirmIdleEarnPanel, AConfirmIdleEarnPanel>
    {
        
    }
    
    public enum TypeConfirmIdleEarnPanel
    {
        LevelUp,
        Unlock,
        UnlockSpecial
    }
}