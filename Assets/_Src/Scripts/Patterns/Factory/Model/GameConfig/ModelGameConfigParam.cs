using Game.Runtime;

namespace Game.Model
{
    public abstract class ModelGameConfigParam
    {
        public TypeGameConfig Type;
    }
    
    public class FactoryModelGameConfigParam : Factory<TypeGameConfig, ModelGameConfigParam>
    {

    }
}