

using Template.Defines;

namespace Game.Data
{
    public interface IResourceItem
    {
        TypeResource Type { get; }
        float Value { get; }
    }
}