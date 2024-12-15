using UnityEngine;
using Cysharp.Threading.Tasks;
using Game.Core;
using System.Threading;
using Template.Utils;

namespace Game.Runtime
{
	[Factory(PetStateType.Thinking)]
	public class PetStateThinking : State<PetStateMachine>
	{
		private CancellationTokenSource _cts;

		public override void Enter(ModelStateData model = null)
		{
			Thinking().Forget();
		}

		public override void Exit()
		{
			_cts?.Cancel();
		}

		private async UniTask Thinking()
		{
			var owner = _context.Owner;
			owner.Animation.Idle();

			_cts = new CancellationTokenSource();
			await UniTask.Delay(1000, cancellationToken: _cts.Token);

			var dice = Random.Range(0, 100);
			if (dice < 50)
			{
				_context.SetState(PetStateType.Move);
			}
			else
			{
				await Thinking();
			}
		}
	}
}