// Author:    -    ad
// Created: 27/07/2024  : : 2:05 AM
// DateUpdate: 27/07/2024

using Game.Runtime;
using Template.Utils;

[Factory(GirlStateType.LevelUp)]
public class GirlStateLevelUp : State<GirlStateMachine>
{
    public override void Enter(ModelStateData model = null)
    {
        var owner = _context.Owner;
        // owner.Animation.ChangeVisual(owner.EntityData.Level);
    }

    public override void Exit()
    {
        _context.Owner.StateMachine.SetState(GirlStateType.Idle);
        base.Exit();
    }
}