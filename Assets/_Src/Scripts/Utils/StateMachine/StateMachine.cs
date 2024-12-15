using System;
using System.Collections.Generic;

namespace Template.Utils
{
	public interface IStateMachine
	{
		void SetState(Enum state, ModelStateData model = null);
		void PushState(Enum state, ModelStateData model = null);
		void PopState(ModelStateData model = null);
		void Update();
		bool IsState(Enum state);

	}

	public abstract class StateMachine<TStateMachine, TOwner> : IStateMachine
	{
		private readonly TOwner _owner;
		private readonly List<State<TStateMachine>> _states = new();
		private readonly Stack<Enum> _histories = new();
		private State<TStateMachine> _currentState;

		public TOwner Owner => _owner;

		public StateMachine(TOwner owner)
		{
			_owner = owner;
		}

		public void AddState(State<TStateMachine> state)
		{
			_states.Add(state);
		}

		public void SetState(Enum state, ModelStateData model = null)
		{
			CleanUp();

			_histories.Push(state);

			_currentState = _states[Convert.ToInt32(state)];
			_currentState.Enter(model);
		}

		public void PushState(Enum state, ModelStateData model = null)
		{
			_currentState?.Exit();
			_currentState = _states[Convert.ToInt32(state)];
			_currentState.Enter(model);

			_histories.Push(state);
		}

		public void PopState(ModelStateData model = null)
		{
			if (IsRoot())
			{
				return;
			}

			_currentState?.Exit();

			_histories.Pop();

			_currentState = _states[Convert.ToInt32(_histories.Peek())];
			_currentState.Enter(model);
		}

		public void Update()
		{
			_currentState?.Update();
		}

		public void Exit()
		{
			CleanUp();
		}

		public bool IsRoot()
		{
			return _histories.Count == 1;
		}

		public bool IsState(Enum state)
		{
			return _currentState == _states[Convert.ToInt32(state)];
		}

		private void CleanUp()
		{
			while (_histories.Count > 1)
			{
				_states[Convert.ToInt32(_histories.Pop())].Exit();
			}
		}
	}
}