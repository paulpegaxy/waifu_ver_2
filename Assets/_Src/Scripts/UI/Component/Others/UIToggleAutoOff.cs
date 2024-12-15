using UnityEngine;
using Doozy.Runtime.UIManager.Components;

namespace Game.UI
{
    [RequireComponent(typeof(UIToggle))]
    public class UIToggleAutoOff : MonoBehaviour
    {
        private UIToggle _toggle;
        private float _duration;

        private void Awake()
        {
            _toggle = GetComponent<UIToggle>();
        }

        private void OnEnable()
        {
            _toggle.OnValueChangedCallback.AddListener(OnValueChanged);
        }

        private void OnDisable()
        {
            _toggle.OnValueChangedCallback.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(bool value)
        {
            if (value)
            {
                _duration = 3f;
            }
            else
            {
                _duration = 0;
            }
        }

        private void Update()
        {
            // if (!TutorialMgr.Instance.IsCompleted(TutorialCategory.Main)) return;

            if (_duration > 0)
            {
                _duration -= Time.deltaTime;
                if (_duration <= 0)
                {
                    _toggle.SetIsOn(false);
                }
            }
        }
    }
}