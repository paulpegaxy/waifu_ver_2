using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Extensions;
using Game.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Game.UI
{
    public abstract class APopupGirlReward : MonoBehaviour
    {
        [SerializeField] protected ItemAvatar itemAvatar;
        [SerializeField] protected Image imgCharName;
        [SerializeField] protected TMP_Text txtDes;
        [SerializeField] protected TMP_Text txtCharName;
        [SerializeField] protected TMP_Text txtTotalBonus;
        [SerializeField] protected Transform posHolderChar;
        [SerializeField] protected PlayableDirector anim;
        [SerializeField] protected GameObject objEffectLight;

        protected Entity _entity;

        protected virtual void OnEnable()
        {
            OnSetData();
        }
        
        protected abstract void OnSetData();

        protected virtual void OnDisable()
        {
            if (_entity != null)
                DestroyImmediate(_entity);
        }
    }
}