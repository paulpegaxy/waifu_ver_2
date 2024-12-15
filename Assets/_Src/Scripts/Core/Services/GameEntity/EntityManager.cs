using System.Collections.Generic;
using Game.Data;
using Template.Gameplay;
using UnityEngine;

public class EntityManager : IEntityManager
{

    private List<GameEntity> entities;

    public EntityManager()
    {
        entities = new();
    }

    public void Add(GameEntity gameEntity)
    {
        entities.Add(gameEntity);
    }
    public void Remove(GameEntity gameEntity)
    {
        entities.Remove(gameEntity);
    }

    public GameEntity CreateEntity(IEntityData entityData)
    {
        // GameEntity entityController = Resources.Load<GameEntity>(GameConsts.PATH_CONTROLLER_ENTITY + entityData.ID);
        GameEntity entityController = entityData.GetController();
        if (entityController == null)
        {
            throw new System.ArgumentException($"Cannot find object for controller path {entityData.ID}");
        }
        GameEntity entity = Object.Instantiate(entityController);
        entity.ApplyData(entityData);
        return entity;
    }

    public void Clear()
    {
        foreach (GameEntity entity in entities)
        {
            if (entity == null) { continue; }
            Object.Destroy(entity.gameObject);
        }
        entities.Clear();
    }

    public IEnumerable<GameEntity> GetEntities()
    {
        foreach (GameEntity entity in entities)
        {
            yield return entity;
        }
    }

    IEnumerable<GameEntity> IEntityManager.GetEntities()
    {
        throw new System.NotImplementedException();
    }
}