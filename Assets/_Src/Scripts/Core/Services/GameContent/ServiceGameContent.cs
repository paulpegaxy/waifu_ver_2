using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Model;
using Template.VfxManager;

namespace Template.Service
{
    public abstract class ServiceGameContent : IVfxContentLoader
    {
        public abstract IEntityData GetEntityData(string id);

        public abstract IVfxConfig GetVfxConfig(string id);
    }
}