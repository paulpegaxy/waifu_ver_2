// Author: ad   -
// Created: 08/08/2024  : : 00:08
// DateUpdate: 08/08/2024

using System;
using BreakInfinity;

namespace Game.Model
{
    
    [Serializable]
    public abstract class ModelApiUpgradeCostParse
    {
        public int cost;
        
        public BigDouble CostParse => BigDouble.Parse(cost.ToString());
    }
}