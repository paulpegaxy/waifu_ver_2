
using UnityEngine;

namespace Game.UI
{
    public abstract class AButtonItemPackage : MonoBehaviour, IButtonItemPackage
    {
        public void Init()
        {
            OnInit();
        }

        protected abstract void OnInit();
    }

}