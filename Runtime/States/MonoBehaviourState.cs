using UnityEngine;

namespace ActionCode.AnimatorStates
{
    /// <summary>
    /// Binds and executes an <see cref="AbstractMonoBehaviourState"/>.
    /// </summary>
    public sealed class MonoBehaviourState : GenericMonoBehaviourState<AbstractMonoBehaviourState>
    {
        [SerializeField] private string stateName = "";

        protected override AbstractMonoBehaviourState FindState()
        {
            // GetComponentInChildren() doesn't have a overriding for a string type.
            // We need to get it manually.

            // First checking on the parent GameObject
            var state = Transform.GetComponent(stateName) as AbstractMonoBehaviourState;
            if (state) return state;

            // Checking on the children GameObject
            foreach (Transform child in Transform)
            {
                state = child.GetComponent(stateName) as AbstractMonoBehaviourState;
                if (state) return state;
            }

            return null;
        }
    }
}