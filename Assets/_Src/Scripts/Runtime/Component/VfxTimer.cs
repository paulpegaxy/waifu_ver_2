using UnityEngine;

namespace Game.Runtime
{
    public class VfxTimer : MonoBehaviour
    {
        [SerializeField] private GameObject objectVfx;
        [SerializeField] private float showTime;
        [SerializeField] private float hideTime;

        private enum State
        {
            Show,
            Hide
        }

        private State _state;
        private float _duration;

        private void OnEnable()
        {
            SetState(State.Show);
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Show:
                    _duration -= Time.deltaTime;
                    if (_duration <= 0)
                    {
                        SetState(State.Hide);
                    }

                    break;

                case State.Hide:
                    _duration -= Time.deltaTime;
                    if (_duration <= 0)
                    {
                        SetState(State.Show);
                    }
                    break;
            }
        }

        private void SetState(State state)
        {
            _state = state;
            switch (_state)
            {
                case State.Show:
                    _duration = showTime;
                    objectVfx.SetActive(true);
                    break;

                case State.Hide:
                    _duration = hideTime;
                    objectVfx.SetActive(false);
                    break;
            }
        }
    }
}