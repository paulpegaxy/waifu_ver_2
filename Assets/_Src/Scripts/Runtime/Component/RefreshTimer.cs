using System;

namespace Game.Runtime
{
	public class RefreshTimer
	{
		private Action _trigger;
		private float _duration;
		private float _durationMax;
		private bool _isLoop;

		public RefreshTimer(Action trigger, float duration, bool isLoop = false)
		{
			_trigger = trigger;
			_duration = duration;
			_durationMax = duration;
			_isLoop = isLoop;
		}

		public void SetDuration(float duration)
		{
			_duration = duration;
		}

		public void Update(float deltaTime)
		{
			if (_duration >= 0)
			{
				_duration -= deltaTime;
				if (_duration <= 0)
				{
					_trigger?.Invoke();
					if (_isLoop)
					{
						_duration = _durationMax;
					}
				}
			}
		}
	}
}