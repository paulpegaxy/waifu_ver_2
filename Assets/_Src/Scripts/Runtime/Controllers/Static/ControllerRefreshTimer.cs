
using System;
using System.Collections.Generic;
using Game.Runtime;
using Template.Defines;

public static class ControllerRefreshTimer
{
	private static readonly Dictionary<RefreshTimerType, RefreshTimer> _timers = new();

	public static void Add(RefreshTimerType type, Action trigger, float duration, bool isLoop = false)
	{
		if (!_timers.ContainsKey(type))
		{
			_timers.Add(type, new RefreshTimer(trigger, duration, isLoop));
		}
	}

	public static void SetDuration(RefreshTimerType type, float duration)
	{
		if (_timers.ContainsKey(type))
		{
			_timers[type].SetDuration(duration);
		}
	}

	public static void Update(float deltaTime)
	{
		foreach (var timer in _timers)
		{
			timer.Value.Update(deltaTime);
		}
	}
}