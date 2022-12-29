using UnityEngine;

namespace ActionCode.AnimatorStates
{
    /// <summary>
    /// State Machine component for an Animator.
    /// <para>It should be placed in the same GameObject that Animator is.</para>
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    [DefaultExecutionOrder(EXECUTION_ORDER)]
    public sealed class AnimatorStateMachine : MonoBehaviour
    {
        [SerializeField, Tooltip("The local Animator component.")]
        private Animator animator;

        /// <summary>
        /// This component execution order.
        /// </summary>
        public const int EXECUTION_ORDER = -1;

        private void Reset() => animator = GetComponent<Animator>();

        private void Awake()
        {
        }
    }
}