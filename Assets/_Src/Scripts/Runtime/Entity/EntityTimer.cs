using System;
using UnityEngine;

namespace Game.Runtime
{
    public class EntityTimer : MonoBehaviour
    {
        public static Action<Pet> OnTimeUp;

        private Pet _pet;
        private float _duration;

        private void Awake()
        {
            _pet = GetComponent<Pet>();
        }

        private void OnDisable()
        {
            GameObject.Destroy(this);
        }

        private void Update()
        {
            if (_duration > 0)
            {
                _duration -= Time.deltaTime;
                if (_duration <= 0)
                {
                    _duration = 0;
                    Remove();
                }
            }
        }

        private void Remove()
        {
            OnTimeUp?.Invoke(_pet);
        }

        public void SetDuration(int duration)
        {
            _duration = duration;
        }
    }
}