using UnityEngine;

namespace ActionCode.AnimatorStates
{
    /// <summary>
    /// Abstract Binder between a <see cref="StateMachineBehaviour"/> and an <see cref="AbstractState"/>.
    /// </summary>
    public abstract class AbstractBinder : StateMachineBehaviour
    {
        [SerializeField, Tooltip("The State name attached in your GameObject.")]
        private string stateName = "";

        public AbstractState State { get; private set; }
        public AnimatorStateMachine StateMachine { get; private set; }

        internal void Initialize(AnimatorStateMachine stateMachine)
        {
            StateMachine = stateMachine;
            State = StateMachine.GetState(stateName);

            if (State == null)
            {
                var error = $"{stateName} was not found on {stateMachine.gameObject.name} GameObject. Attach one.";
                Debug.LogError(error);
            }
        }
    }
}