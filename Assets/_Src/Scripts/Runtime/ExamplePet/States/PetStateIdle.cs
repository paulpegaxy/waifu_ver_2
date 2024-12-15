using System.Threading;
using Game.Core;
using Template.Utils;

namespace Game.Runtime
{
	[Factory(PetStateType.Idle)]
	public class PetStateIdle : State<PetStateMachine>
	{
		private CancellationTokenSource _cts;

		public override void Enter(ModelStateData model = null)
		{
			var owner = _context.Owner;
			owner.Animation.Idle();
		}
	}
}