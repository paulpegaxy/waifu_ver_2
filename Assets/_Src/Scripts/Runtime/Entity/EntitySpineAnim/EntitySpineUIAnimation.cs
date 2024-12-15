// Author: 
// Created Date: 26/07/2024
// Update Time: 26/07

using Game.Runtime;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

public enum TypeGirlReact
{
    None,
    GirlHead,
    GirlBoob,
    GirlOther
}

public abstract class EntitySpineUIAnimation
{
    private GameObject _animContainer;

    protected SkeletonGraphic Anim;

    protected ISpineView View;
        
    public EntitySpineUIAnimation(GameObject animContainer)
    {
        View = animContainer.GetComponent<ISpineView>();
        Anim = animContainer.GetComponent<SkeletonGraphic>();
        this.Anim.AnimationState.Complete += this.CompleteAnimation;
        this.Anim.AnimationState.Event += this.HandleEvent;
        this.Anim.AnimationState.End += this.EndAnimation;
    }

    protected abstract void EndAnimation(TrackEntry state);

    protected abstract void CompleteAnimation(TrackEntry state);

    protected abstract void HandleEvent(TrackEntry state, Event e);

    public abstract bool IsAnimIdle();
    

    public abstract void Idle();

    public abstract void React(TypeGirlReact typeReact);

    protected void ClearTrack(int track)
    {
        this.Anim.AnimationState.ClearTrack(track);
    }

    protected void ClearAllTracks()
    {
        this.Anim.AnimationState.ClearTracks();
    }
        
    protected void SetToSetupPose()
    {
        this.Anim.Skeleton.SetToSetupPose();
    }

    protected void SetSlotsToSetupPose()
    {
        this.Anim.Skeleton.SetSlotsToSetupPose();
    }
    
    private string GetCurrentAnimation(int trackIndex = 0)
    {
        var entry = Anim.AnimationState.GetCurrent(trackIndex);
        return entry == null ? string.Empty : entry.Animation.Name;
    }
    
    protected bool CheckAnimation(string name, int trackIndex = 0)
    {
        return this.GetCurrentAnimation(trackIndex).Equals(name);
    }

    public void SetBonesToSetupPose()
    {
        this.Anim.Skeleton.SetBonesToSetupPose();
    }
}