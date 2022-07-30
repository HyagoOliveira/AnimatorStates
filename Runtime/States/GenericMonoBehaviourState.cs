using UnityEngine;

namespace ActionCode.AnimatorStates
{
    /// <summary>
    /// Binds and executes a component of type <see cref="AbstractMonoBehaviourState"/>.
    /// </summary>
    public class GenericMonoBehaviourState<T> : AbstractAnimatorState
        where T : AbstractMonoBehaviourState
    {
        private T state;

        protected override void LoadComponents()
        {
            base.LoadComponents();

            state = FindState();
            if (state == null) Debug.LogError($"State {typeof(T).Name} was not found on {GameObject.name}.");
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            state.StateEnter();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            state.StateUpdate();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            state.StateExit();
        }

        protected virtual T FindState() => Transform.GetComponentInChildren<T>(includeInactive: true);
    }
}