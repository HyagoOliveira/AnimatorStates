using UnityEngine;

namespace ActionCode.AnimatorStates
{
    /// <summary>
    /// Binds an Animator State Machine and an <see cref="AbstractState"/>.
    /// </summary>
    public sealed class StateMachineBinder : AbstractBinder
    {
        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            base.OnStateMachineEnter(animator, stateMachinePathHash);
            StateMachine.EnterState(State, 0);
        }

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            base.OnStateMachineExit(animator, stateMachinePathHash);
            StateMachine.ExitState(State, 0);
        }
    }
}