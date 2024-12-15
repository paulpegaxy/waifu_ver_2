// Author: 
// Created Date: 26/07/2024
// Update Time: 26/07

using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Runtime;
using Spine;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;
using Event = Spine.Event;

public class GirlSpineAnimation : EntitySpineUIAnimation
{
    public GirlSpineView GirlSpineView => (GirlSpineView) View;

    private int FILL_PHASE_ID => Shader.PropertyToID("_FillPhase");
    
    private bool _isReacting;
    
    public GirlSpineAnimation(GameObject animContainer) : base(animContainer)
    {
        _isReacting = false;
        Anim.material.SetFloat(FILL_PHASE_ID, 0f);
    }

    protected override void EndAnimation(TrackEntry state)
    {
        var track = state.TrackIndex;
        var nameAnim = state.Animation.Name;
        // Debug.LogError("EndAnimation ne: "+nameAnim);
        
        // switch (track)
        // {
        //
        //     default:
        //         if (!state.Loop && state.Next == null)
        //         {
        //             // ClearTrack(0);
        //             // SetSlotsToSetupPose();
        //             Anim.AnimationState.SetAnimation(0, GirlSpineView.animIdle, true);
        //             _isReacting = false;
        //
        //         }
        //         break;
        // }
    }

    protected override void CompleteAnimation(TrackEntry state)
    {
        var track = state.TrackIndex;
        var nameAnim = state.Animation.Name;
        // Debug.LogError("CompleteAnimation Anim: "+nameAnim);
        switch (track)
        {
        
            default:
                if (!state.Loop && state.Next == null)
                {
                    // ClearTrack(0);
                    // SetSlotsToSetupPose();
                     // Anim.AnimationState.SetAnimation(0, GirlSpineView.animIdle, true);
                    _isReacting = false;
        
                }
                break;
        }
    }

    protected override void HandleEvent(TrackEntry state, Event e)
    {
            
    }

    public override void Idle()
    {
        // ClearAllTracks();
        // ClearTrack(1);
        Anim.AnimationState.SetAnimation(0, GirlSpineView.animIdle, true);
    }

    public override void React(TypeGirlReact typeReact)
    {
        if (_isReacting)
            return;

        _isReacting = true;
        
        var trackEntry = typeReact switch
        {
            TypeGirlReact.GirlHead => Anim.AnimationState.SetAnimation(1, GirlSpineView.animReactHead, false),
            TypeGirlReact.GirlBoob => Anim.AnimationState.SetAnimation(1, GirlSpineView.animReactBoob, false),
            _ => Anim.AnimationState.SetAnimation(1, GirlSpineView.animReactPussy, false)
        };
        trackEntry.AnimationEnd = trackEntry.Animation.Duration;

    }
    
    public override bool IsAnimIdle()
    {
        return CheckAnimation(GirlSpineView.animIdle);
    }

    public void ChangeVisual(int level)
    {
        var modValue = level % GirlSpineView.listSkin.Count;
        //Find index in list skin
        int index = modValue - 1;
       
        if (index < 0)
        {
            index = GirlSpineView.listSkin.Count - 1;
        }

        var skin = GirlSpineView.listSkin[index];
        
        if (GirlSpineView.currentSkin == skin)
        {
            return;
        }
        
        Anim.Skeleton.SetSkin(skin);
        SetSlotsToSetupPose();
        GirlSpineView.currentSkin = skin;
    }

    public void Undress(int level)
    {
        Anim.material.SetFloat(FILL_PHASE_ID, 1f);
        ChangeVisual(level);
    }

    public async UniTask DoneUndress(float duration)
    {
        DOTween.To(() => Anim.material.GetFloat(FILL_PHASE_ID), x =>
            Anim.material.SetFloat(FILL_PHASE_ID, x), 0f, duration);
        await UniTask.Delay((int)(duration * 1000));
    }
}