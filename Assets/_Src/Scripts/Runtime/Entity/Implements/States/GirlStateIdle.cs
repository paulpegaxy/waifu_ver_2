// Author: 
// Created Date: 26/07/2024
// Update Time: 26/07

using System.Threading;
using Game.Runtime;
using Template.Utils;

[Factory(GirlStateType.Idle)]
public class GirlStateIdle : State<GirlStateMachine>
{
    private CancellationTokenSource _cts;

    public override void Enter(ModelStateData model = null)
    {
        var owner = _context.Owner;
        owner.Animation.Idle();
    }
}