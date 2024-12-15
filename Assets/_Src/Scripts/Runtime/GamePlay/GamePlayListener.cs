using Game.Extensions;
using UnityEngine;
using Game.Model;
using Template.Defines;

namespace Game.Runtime
{
    public abstract class GameplayListener : MonoBehaviour
    {
        protected ModelApiGameInfo DataGameInfo;
        
        protected abstract void OnGameInfoChanged(ModelApiGameInfo gameInfo);

        protected virtual void OnEnable()
        {
            var apiGameInfo = FactoryApi.Get<ApiGame>().Data.Info;
            if (apiGameInfo==null) return;
            
            ModelApiGameInfo.OnChanged += OnChanged;
            OnInitialize();
        }



        protected virtual void OnDisable()
        {
            ModelApiGameInfo.OnChanged -= OnChanged;
        }

        protected virtual void OnInitialize()
        {
            
        }

        private void OnChanged(ModelApiGameInfo gameInfo)
        {
            DataGameInfo = gameInfo;
            OnGameInfoChanged(gameInfo);
        }

    }
}