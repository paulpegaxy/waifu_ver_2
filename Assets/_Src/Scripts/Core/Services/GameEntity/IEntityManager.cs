using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Template.Gameplay;

public interface IEntityManager
{
    IEnumerable<GameEntity> GetEntities();
    void Add(GameEntity gameEntity);
    void Remove(GameEntity gameEntity);
    GameEntity CreateEntity(IEntityData entityData);
    void Clear();
}