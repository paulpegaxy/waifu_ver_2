using Game.Data;
using Template.Defines;
using UnityEngine;

namespace Template.Gameplay
{
    public class GameEntity : MonoBehaviour
    {
        public virtual bool AbleToLevelUp()
        {
            return true;
        }
        public virtual bool CompareTo(GameEntity entity)
        {
            return true;
        }

        public virtual void ApplyData(IEntityData data)
        {

        }

        public virtual TypeEntity GetEntityKind()
        {
            return TypeEntity.Undefined;
        }

        public virtual void SetLevel(int level)
        {

        }
        public virtual int GetLevel()
        {
            return 0;
        }
        public virtual void LevelUp()
        {

        }

        public virtual IEntityData GetData()
        {
            return null;
        }

        public virtual void BattleUpdate()
        {

        }
    }
}