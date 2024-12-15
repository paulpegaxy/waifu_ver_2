
using Game.Runtime;
using Spine.Unity;
using UnityEngine;

public class TapGirlVfxControl : MonoBehaviour
{
    [SerializeField] private SkeletonGraphic anim; 
    [SpineAnimation,SerializeField] private string animClickTop;
    [SpineAnimation, SerializeField] private string animClickOther;

    public void PlayAnim(TypeGirlReact typeGirlReact)
    {
        string animName = typeGirlReact == TypeGirlReact.GirlHead ? animClickTop : animClickOther;
        var track = anim.AnimationState.SetAnimation(0, animName, false);
        this.StartDelayMethod(track.AnimationEnd, () =>
        {
            ControllerSpawner.Instance.Return(gameObject);
        });
    }
}
