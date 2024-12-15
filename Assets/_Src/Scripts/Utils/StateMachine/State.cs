namespace Template.Utils
{
    public interface IState
    {
        void Enter(ModelStateData model = null);
        void Update();
        void Exit();
    }

    public abstract class State<TStateMachine> : IState
    {
        protected TStateMachine _context;

        public void Bind(TStateMachine context)
        {
            _context = context;
        }

        public virtual void Enter(ModelStateData model = null) { }
        public virtual void Update() { }
        public virtual void Exit() { }
    }

    public class ModelStateData
    {
    }
}