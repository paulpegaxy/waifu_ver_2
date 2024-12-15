using Game.Core;
using Game.Runtime;
using Template.Defines;

namespace Game.Model
{
    public class ModelEntity
    {
        public int Id;
        public TypeEntity Type;
    }

    public class FactoryModelEntity : Factory<TypeEntity, ModelEntity>
    {
    }
}