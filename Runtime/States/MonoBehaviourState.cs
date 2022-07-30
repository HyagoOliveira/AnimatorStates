using UnityEngine;

namespace ActionCode.AnimatorStates
{
    /// <summary>
    /// Binds and executes an <see cref="AbstractMonoBehaviourState"/>.
    /// </summary>
    public sealed class MonoBehaviourState : AbstractAnimatorState
    {
        [SerializeField] private string stateName = "";

        private AbstractMonoBehaviourState state;

        protected override void LoadComponents()
        {
            base.LoadComponents();

            state = GameObject.GetComponent(stateName) as AbstractMonoBehaviourState;
            if (state == null) Debug.LogError($"State {stateName} was not found on {GameObject.name}.");
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
    }
}