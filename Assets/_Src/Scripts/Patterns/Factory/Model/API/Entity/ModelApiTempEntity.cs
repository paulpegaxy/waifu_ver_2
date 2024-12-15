// Author: ad   -
// Created: 14/12/2024  : : 21:12
// DateUpdate: 14/12/2024

using System;
using System.Collections.Generic;

namespace Game.Model
{
    [Serializable]
    public class ModelApiTempEntity
    {
        public List<ModelApiEntityConfig> sortedCharacters;
        public List<ModelApiEntityExpConfig> configExpRequire;
    }
}