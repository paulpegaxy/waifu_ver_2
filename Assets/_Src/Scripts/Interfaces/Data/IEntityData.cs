using System.Reflection;
using Template.Gameplay;
using UnityEngine;

namespace Game.Data
{
    public interface IEntityData
    {
        string ID { get; }
        string Name { get; }
        string ControllerName { get; }
        GameEntity GetController();
    }
}