using System;
using System.Collections.Generic;

namespace Template.Utils
{

    public class GameStateMachine<TState> where TState : Enum
    {
        private Dictionary<TState, IGameState<TState>> states;

        private IGameState<TState> _currentGameState;

        public GameStateMachine()
        {
            states = new();
        }

        public void Update()
        {
            if (_currentGameState == null) { return; }
            _currentGameState.Update();
        }

        public void AddState(TState stateType, IGameState<TState> gameState)
        {
            gameState.SetMachine(this);
            if (states.ContainsKey(stateType))
            {
                states[stateType] = gameState;
            }
            else
            {
                states.Add(stateType, gameState);
            }
        }

        public void SwitchState(TState nextState)
        {
            if (!states.ContainsKey(nextState)) { return; }
            if (_currentGameState != null)
            {
                _currentGameState.Exit();
            }
            _currentGameState = states[nextState];
            _currentGameState.Enter();
        }
    }
}