
using System;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Game.Defines;
using Game.Model;
using Game.Runtime;
using I2.Loc;
using Template.Defines;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class SettingPanelTabConfig : MonoBehaviour
    {
        [SerializeField] private UIToggle toggleMusic;
        [SerializeField] private UIToggle toggleSound;
        [SerializeField] private UIToggle toggleQuality;
        [SerializeField] private UIToggleGroup toggleQualitySelect;
        [SerializeField] private TMP_Text textQuality;
        [SerializeField] private ItemSelectLanguage selectLanguage;
        
        private StorageSettings _storageSetting;
        private ModelStorageSetting _model;

        private void OnEnable()
        {
            _storageSetting = FactoryStorage.Get<StorageSettings>();
            _model = _storageSetting.Get();
            
            toggleMusic.SetIsOn(_model.isMusic, false, false);
            toggleSound.SetIsOn(_model.isSound, false, false);
            
            if (!LocalizationManager.CurrentLanguage.Equals(_model.language.ToString()))
            {
                _model.language = LocalizationManager.CurrentLanguage.ToEnum<TypeLanguage>();
                _storageSetting.Save();
            }
            
            selectLanguage.Select(_model.language, false, true);
            
            
            toggleMusic.OnValueChangedCallback.AddListener(OnMusic);
            toggleSound.OnValueChangedCallback.AddListener(OnSound);
            selectLanguage.OnSelected += OnLanguage;
        }

        private void OnDisable()
        {
            toggleMusic.OnValueChangedCallback.RemoveListener(OnMusic);
            toggleSound.OnValueChangedCallback.RemoveListener(OnSound);
            selectLanguage.OnSelected -= OnLanguage;
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
        
        
        private void OnLanguage(TypeLanguage language)
        {
            LocalizationManager.CurrentLanguage = language.ToString();
            // selectQuality.Select(_model.Quality, false, false);

            _model.language = language;
            _storageSetting.Save();
        }
    }
}