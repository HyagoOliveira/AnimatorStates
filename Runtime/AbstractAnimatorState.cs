using UnityEngine;

namespace ActionCode.AnimatorStates
{
    /// <summary>
    /// Abstract State for an Animator.
    /// </summary>
    public abstract class AbstractAnimatorState : StateMachineBehaviour
    {
        /// <summary>
        /// The State Machine attached to this State.
        /// </summary>
        public AnimatorStateMachine StateMachine { get; private set; }

        /// <summary>
        /// <inheritdoc cref="Component.transform"/>
        /// </summary>
        public Transform Transform => StateMachine.transform;

        /// <summary>
        /// <inheritdoc cref="Component.gameObject"/>
        /// </summary>
        public GameObject GameObject => StateMachine.gameObject;

        internal void Initialize(AnimatorStateMachine stateMachine)
        {
            StateMachine = stateMachine;

            LoadComponents();
            LoadParameters();
        }

        /// <summary>
        /// Similar to MonoBehaviour.Start(), this function is called on the frame when a 
        /// script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// Similar to MonoBehaviour.Update(), this function is called every frame, 
        /// if the <see cref="StateMachine"/> is enabled.
        /// </summary>
        public virtual void Update() { }

        protected virtual void LoadParameters() { }
        protected virtual void LoadComponents() { }

        #region Override Components functions
        /// <summary>
        /// <inheritdoc cref="AnimatorStateMachine.GetBehaviour{T}"/>
        /// </summary>
        protected T GetBehaviour<T>() where T : StateMachineBehaviour => StateMachine.GetBehaviour<T>();

        /// <summary>
        /// <inheritdoc cref="GameObject.GetComponent"/>
        /// </summary>
        protected T GetComponent<T>() where T : Component => Transform.GetComponent<T>();

        /// <summary>
        /// <inheritdoc cref="GameObject.GetComponents"/>
        /// </summary>
        protected T[] GetComponents<T>() where T : Component => Transform.GetComponents<T>();

        /// <summary>
        /// <inheritdoc cref="GameObject.GetComponentInChildren"/>
        /// </summary>
        protected T GetComponentInChildren<T>() where T : Component => Transform.GetComponentInChildren<T>();

        /// <summary>
        /// <inheritdoc cref="GameObject.GetComponentsInChildren"/>
        /// </summary>
        protected T[] GetComponentsInChildren<T>(bool includeInactive = false) where T : Component =>
            Transform.GetComponentsInChildren<T>(includeInactive);

        /// <summary>
        /// <inheritdoc cref="MonoBehaviour.print(object)"/>
        /// </summary>
        protected void Print(object message) => MonoBehaviour.print(message);
        #endregion
    }
}