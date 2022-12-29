using UnityEngine;

namespace ActionCode.AnimatorStates
{
    /// <summary>
    /// Binds an Animator State and an <see cref="AbstractState"/>.
    /// </summary>
    public sealed class StateBinder : AbstractBinder
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            StateMachine.EnterState(State, layerIndex);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            StateMachine.UpdateState(State);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            StateMachine.ExitState(State, layerIndex);
        }
    }
}