// Author: 
// Created Date: 26/07/2024
// Update Time: 26/07

using System;
using Game.Runtime;
using Template.Utils;

public enum GirlStateType
{
    Idle,
    React,
    LevelUp
}

public class GirlStateMachine : StateMachine<GirlStateMachine, GirlEntity>
{
    public GirlStateMachine(GirlEntity owner) : base(owner)
    {
        int count = Enum.GetNames(typeof(GirlStateType)).Length;
        for (int i = 0; i < count; i++)
        {
            State<GirlStateMachine> state = FactoryGirlState.Get<IState>((GirlStateType)i) as State<GirlStateMachine>;
            state.Bind(this);

            AddState(state);
        }
    }
}

public class FactoryGirlState : Factory<GirlStateType, IState>
{
}