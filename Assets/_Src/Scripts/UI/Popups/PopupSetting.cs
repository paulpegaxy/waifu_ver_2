using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Doozy.Runtime.UIManager.Components;
using Game.Runtime;
using Game.Model;
using Template.Defines;

namespace Game.UI
{
    public class PopupSetting : MonoBehaviour
    {
        [SerializeField] private UIToggle toggleMusic;
        [SerializeField] private UIToggle toggleSound;
        [SerializeField] private UIToggle toggleQuality;
        [SerializeField] private UIToggleGroup toggleQualitySelect;
        [SerializeField] private TMP_Text textQuality;

        private StorageSettings _storageSetting;
        private ModelStorageSetting _model;

        private void Start()
        {
            _storageSetting = FactoryStorage.Get<StorageSettings>();
            _model = _storageSetting.Get();

            toggleMusic.SetIsOn(_model.isMusic, false, false);
            toggleSound.SetIsOn(_model.isSound, false, false);
            toggleQualitySelect.toggles[(int)_model.quality].SetIsOn(true, false, false);
            SetQualityText(_model.quality);

            toggleMusic.OnValueChangedCallback.AddListener(OnMusic);
            toggleSound.OnValueChangedCallback.AddListener(OnSound);
            toggleQualitySelect.OnToggleTriggeredCallback.AddListener(OnQuality);
        }

        private void OnDestroy()
        {
            toggleMusic.OnValueChangedCallback.RemoveListener(OnMusic);
            toggleSound.OnValueChangedCallback.RemoveListener(OnSound);
            toggleQualitySelect.OnToggleTriggeredCallback.RemoveListener(OnQuality);
        }

        private void OnMusic(bool isOn)
        {
            if (isOn)
            {
                ControllerAudio.Instance.Unmute(AudioMixerType.Bgm);
            }
            else
            {
                ControllerAudio.Instance.Mute(AudioMixerType.Bgm);
            }

            _model.isMusic = isOn;
            _storageSetting.Save();
        }

        private void OnSound(bool isOn)
        {
            if (isOn)
            {
                ControllerAudio.Instance.Unmute(AudioMixerType.Sfx);
            }
            else
            {
                ControllerAudio.Instance.Mute(AudioMixerType.Sfx);
            }

            _model.isSound = isOn;
            _storageSetting.Save();
        }

        private void OnQuality(UIToggle toggle)
        {
            var quality = (TypeQuality)toggleQualitySelect.lastToggleOnIndex;
            switch (quality)
            {
                case TypeQuality.Low:
                    Application.targetFrameRate = 30;
                    break;

                case TypeQuality.High:
                    Application.targetFrameRate = 60;
                    break;
            }

            toggleQuality.SetIsOn(false);
            SetQualityText(quality);

            _model.quality = quality;
            _storageSetting.Save();
        }

        private void SetQualityText(TypeQuality quality)
        {
            var textIds = new List<TextId>()
            {
                TextId.Common_Low,
                TextId.Common_High,
            };

            textQuality.text = Localization.Get(textIds[(int)quality]);
        }
    }
}