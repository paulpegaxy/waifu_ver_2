using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core;
using Game.Model;
using Template.Utils;

namespace Game.Runtime
{
	[Factory(PetStateType.Move)]
	public class PetStateScan : State<PetStateMachine>
	{
		private CancellationTokenSource _cts;

		public override void Enter(ModelStateData model = null)
		{
			_context.SetState(PetStateType.Thinking);
		}

		public override void Exit()
		{
			_cts?.Cancel();
		}
	}
}