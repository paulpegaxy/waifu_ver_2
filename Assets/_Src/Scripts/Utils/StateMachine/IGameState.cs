using System;

namespace Template.Utils
{
    public interface IGameState<TState> where TState : Enum
    {
        GameStateMachine<TState> Machine { get; }
        void SetMachine(GameStateMachine<TState> machine);
        void Enter();
        void Update();
        void Exit();
    }
}