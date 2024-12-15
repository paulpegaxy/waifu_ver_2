
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Runtime
{
	public static class ControllerGameAudio
	{
		private static CancellationTokenSource _tokenSource;
		private static float _mergeComboDuration;
		private static int _mergeIndex;

		static ControllerGameAudio()
		{
			_tokenSource = new CancellationTokenSource();
			PlayerLoopTimer.StartNew(TimeSpan.FromSeconds(0.1f), true, DelayType.DeltaTime, PlayerLoopTiming.Update, _tokenSource.Token, Update, null);
		}

		private static void Update(object data)
		{
			if (_mergeComboDuration > 0)
			{
				_mergeComboDuration -= 0.1f;
				if (_mergeComboDuration <= 0)
				{
					_mergeIndex = 0;
				}
			}
		}
	}
}