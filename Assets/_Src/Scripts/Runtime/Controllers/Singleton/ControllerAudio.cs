using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Game.Core;
using Template.Defines;
using Template.UI;
using Unity.Collections;

namespace Game.Runtime
{
	public class ControllerAudio : Singleton<ControllerAudio>
	{
		[SerializeField] private AudioMixer audioMixer;

		// [SerializeField] private AudioClip bgMusic;
		[SerializeField] private List<AudioMixerGroup> audioMixerGroups;

		const string UISelectable = nameof(UISelectable);
		const string UIButton = nameof(UIButton);
		const string UIToggle = nameof(UIToggle);

		private List<AudioSource> _audioSources = new();
		private Queue<AudioClip> _queueAudioGirl = new();

		public int CountAudioGirl => _queueAudioGirl.Count;

		private string _prevToggleName;

		void OnEnable()
		{
			SignalsService.OnSignal += OnSignal;
		}

		void OnDisable()
		{
			SignalsService.OnSignal -= OnSignal;
		}

		private void OnSignal(Signal signal)
		{
			if (signal.stream.category == UISelectable)
			{
				if (signal.stream.name == UIButton)
				{
					var key = AnR.AudioKey.UiTap;
					var data = (UIButtonSignalData)signal.valueAsObject;
					var sfx = data.button.GetComponent<CustomSfx>();
					if (sfx)
					{
						key = sfx.AudioKey;
					}

					PlaySfx(key);
				}
				else if (signal.stream.name == UIToggle)
				{
					var key = AnR.AudioKey.UiTap;
					var data = (UIToggleSignalData)signal.valueAsObject;

					if (data.state == CommandToggle.On)
					{
						// if (data.toggle!=null && data.toggle.name == _prevToggleName)
						// {
						// 	return;
						// }
							
						var sfx = data.toggle.GetComponent<CustomSfx>();
						if (sfx)
						{
							key = sfx.AudioKey;
						}

						_prevToggleName = data.toggle.name;
						PlaySfx(key);
					}
				}
			}
		}

		private void OnItemChanged(AudioMixerType type, bool value)
		{
			switch (type)
			{
				case AudioMixerType.Bgm:
					audioMixer.SetFloat(AudioMixerType.Bgm.ToString(), value ? 0 : -80);
					break;

				case AudioMixerType.Sfx:
					audioMixer.SetFloat(AudioMixerType.Sfx.ToString(), value ? 0 : -80);
					break;
			}
		}

		private AudioSource GetAudioSource()
		{
			var audioSource = _audioSources.Find(x => !x.isPlaying);
			if (audioSource == null)
			{
				audioSource = gameObject.AddComponent<AudioSource>();
				_audioSources.Add(audioSource);
			}

			return audioSource;
		}

		private AudioSource Get(AnR.AudioKey key, AudioMixerType mixer, bool loop = false)
		{
			var clip = AnR.Get<AudioClip>(key);
			if (clip != null)
			{
				var audioSource = GetAudioSource();
				audioSource.clip = clip;
				audioSource.loop = loop;
				audioSource.outputAudioMixerGroup = audioMixerGroups[(int)mixer];
				if (key == AnR.AudioKey.TapGirl)
				{
					audioSource.volume = 0.1f;
					// audioSource.volume = 1f;
				}
				else
				{
					audioSource.volume = 1;
				}

				return audioSource;
			}

			return null;
		}
		
		private AudioSource Get(AudioClip clip, AudioMixerType mixer, bool loop = false)
		{
			if (clip != null)
			{
				var audioSource = GetAudioSource();
				audioSource.clip = clip;
				audioSource.loop = loop;
				audioSource.outputAudioMixerGroup = audioMixerGroups[(int)mixer];

				return audioSource;
			}

			return null;
		}

		public AudioSource PlayBgm(AnR.AudioKey key, bool loop = true)
		{
			StopAllBgm();

			var audioSource = Get(key, AudioMixerType.Bgm, loop);
			audioSource.Play();

			return audioSource;
		}

		public AudioSource PlayDefaultBgm()
		{
			var audioSource = GetAudioSource();
			// audioSource.clip = bgMusic;
			audioSource.loop = true;
			audioSource.outputAudioMixerGroup = audioMixerGroups[(int)AudioMixerType.Bgm];
			return audioSource;
		}

		public AudioSource PlaySfx(AnR.AudioKey key, bool loop = false)
		{
			var audioSource = Get(key, AudioMixerType.Sfx, loop);
			if (audioSource != null)
			{
				audioSource.Play();
			}
			return audioSource;
		}

		public void ProcessQueueAudioGirl(List<AudioClip> listAudio)
		{
			_queueAudioGirl = new Queue<AudioClip>(listAudio);
		}

		public void PlayGirlAudio()
		{
			var audioClip = _queueAudioGirl.Dequeue();
			// UnityEngine.Debug.LogError("Vao day: " + audioClip.name + ", con lai: " + _queueAudioGirl.Count);
			var audioSource = Get(audioClip, AudioMixerType.Sfx, false);
			if (audioSource != null)
			{
				audioSource.Play();
			}
		}

		public void StopAllBgm()
		{
			foreach (var audioSource in _audioSources)
			{
				if (audioSource != null && audioSource.loop && audioSource.outputAudioMixerGroup == audioMixerGroups[(int)AudioMixerType.Bgm])
				{
					audioSource.Stop();
				}
			}
		}

		public void StopAllSfx()
		{
			foreach (var audioSource in _audioSources)
			{
				if (audioSource != null && audioSource.loop &&
					audioSource.outputAudioMixerGroup == audioMixerGroups[(int)AudioMixerType.Sfx])
				{
					audioSource.Stop();
				}
			}
		}

		public void Mute(AudioMixerType type)
		{
			audioMixer.SetFloat(type.ToString(), -80);
		}

		public void Unmute(AudioMixerType type)
		{
			audioMixer.SetFloat(type.ToString(), 0);
		}
	}
}