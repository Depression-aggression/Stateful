using System.Runtime.CompilerServices;
using Depra.Stateful.Abstract;

namespace Depra.Stateful.Finite
{
	public abstract class StateMachine<TState> : IStateMachine<TState>
	{
		private readonly bool _allowReentry;

		public event IStateMachine<TState>.StateChangedDelegate StateChanged;

		protected StateMachine(bool allowReentry = false) =>
			_allowReentry = allowReentry;

		protected StateMachine(TState startingState, bool allowReentry = false) : this(allowReentry) =>
			SwitchState(startingState);

		public TState CurrentState { get; private set; }

		public void SwitchState(TState to)
		{
			if (CanEnterState(to) == false)
			{
				return;
			}

			OnStateExit(CurrentState);
			CurrentState = to;
			OnStateEnter(CurrentState);
		}

		protected virtual void OnStateExit(TState state) { }

		protected virtual void OnStateEnter(TState state) { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool CanEnterState(TState state) =>
			_allowReentry || Equals(CurrentState, state) == false;
	}
}