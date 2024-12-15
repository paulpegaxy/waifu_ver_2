using System;
using UnityEngine;
using Game.Model;

namespace Game.Runtime
{
    public abstract class Entity : MonoBehaviour
    {
        public ModelEntity Model { get; protected set; }

        public virtual void Init(int id,Action onComplete=null) { }
        public virtual void InitToShowReward(int id,Action onComplete=null){}
        public virtual void Reset() { }

        protected GameObject AnimContainer;

        protected int CurrentEntityId;

        public virtual void DestroyObject()
        {
            
        }
        
        protected virtual void SetData(int id)
        {
            CurrentEntityId = id;
        }
    }
}