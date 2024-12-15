using System;
using Game.Core;
using Template.Utils;

namespace Game.Runtime
{
    public enum PetStateType
    {
        Idle,
        Move,
        Thinking
    }

    public class PetStateMachine : StateMachine<PetStateMachine, Pet>
    {
        public PetStateMachine(Pet owner) : base(owner)
        {
            int count = Enum.GetNames(typeof(PetStateType)).Length;
            for (int i = 0; i < count; i++)
            {
                State<PetStateMachine> state = FactoryPetState.Get<IState>((PetStateType)i) as State<PetStateMachine>;
                state.Bind(this);

                AddState(state);
            }
        }
    }

    public class FactoryPetState : Factory<PetStateType, IState>
    {
    }
}