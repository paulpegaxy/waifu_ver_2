// Author: 
// Created Date: 26/07/2024
// Update Time: 26/07

using Game.Runtime;
using Template.Utils;

[Factory(GirlStateType.React)]
public class GirlStateReact : State<GirlStateMachine>
{
 
    public override void Enter(ModelStateData model = null)
    {
        var owner = _context.Owner;
        owner.Animation.React(owner.TypeReact);
    }
}