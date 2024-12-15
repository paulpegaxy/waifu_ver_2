// Author:    -    ad
// Created: 22/07/2024  : : 11:25 PM
// DateUpdate: 22/07/2024

using DG.Tweening;
using UnityEngine;

namespace Game.UI
{
    [RequireComponent(typeof(DOTweenAnimation))]
    public class ItemAnimDOTween : MonoBehaviour
    {
        private DOTweenAnimation _doTweenAnim;
        
        protected DOTweenAnimation DoTweenAnim
        {
            get
            {
                if (_doTweenAnim == null)
                {
                    _doTweenAnim = GetComponent<DOTweenAnimation>();
                }

                return _doTweenAnim;
            }
        }
    }
}