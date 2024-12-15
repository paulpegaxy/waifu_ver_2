using System.Collections;
using System.Collections.Generic;
using Template.Utils;
using UnityEngine;

public enum TypeGameState
{
    None,
    Battle,
    End,
    GameOver
}

public class GameGameState : IGameState<TypeGameState>
{
    public GameStateMachine<TypeGameState> Machine { get; private set; }


    public void SetMachine(GameStateMachine<TypeGameState> machine)
    {
        this.Machine = machine;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void Update()
    {

    }
}