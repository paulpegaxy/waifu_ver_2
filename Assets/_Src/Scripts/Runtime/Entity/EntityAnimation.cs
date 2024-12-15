using UnityEngine;

namespace Game.Runtime
{
    public class EntityAnimation
    {
        private enum Animation
        {
            Idle,
            Move,
        }

        private Entity _owner;
        private Animator _animator;

        public EntityAnimation(Entity owner)
        {
            _owner = owner;
            _animator = owner.GetComponent<Animator>();
        }

        public void Init(Animator animator)
        {
            _animator = animator;
        }

        public void Idle()
        {
            _animator.SetTrigger(Animation.Idle.ToString());
        }

        public void Move()
        {
            _animator.SetTrigger(Animation.Move.ToString());
        }
    }
}