using System;
using System.Collections.Generic;
using System.Linq;
using Game.Data;
using Game.Model;
using Game.Runtime;
using Template.VfxManager;
using UnityEngine;

namespace Template.Service
{
    public class FactoryContentManager : ServiceGameContent
    {
        public override IEntityData GetEntityData(string id)
        {
            throw new 
                NotImplementedException();
        }

        public override IVfxConfig GetVfxConfig(string id)
        {
            throw new NotImplementedException();
        }
    }
}