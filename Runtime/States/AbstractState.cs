using System;
using System.Collections;
using UnityEngine;

namespace ActionCode.AnimatorStates
{
    /// <summary>
    /// Abstract component implementing <see cref="IState"/>.
    /// </summary>
    public abstract class AbstractState : MonoBehaviour, IState, IEquatable<AbstractState>
    {
        public event Action OnEnter;
        public event Action OnUpdate;
        public event Action OnExit;

        public AnimatorStateMachine StateMachine { get; internal set; }

        public bool Enabled => gameObject.activeInHierarchy && enabled;
        public bool IsExecuting { get; private set; }

        public ulong TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }

        protected virtual void Reset() => CheckStateMachine();

        protected virtual void Start()
        {
            // To enable the component toggle in the Inspector.
        }

        /// <summary>
        /// Returns this State name.
        /// </summary>
        /// <returns>A String containing this State name.</returns>
        public override string ToString() => GetType().Name;

        /// <summary>
        /// Gets information about this State.
        /// </summary>
        /// <returns>A String containing this information about this State.</returns>
        public virtual string GetInfo() => $"{this} (F: {TotalFrames})";

        /// <summary>
        /// Checks whether this State is equals to the given one.
        /// </summary>
        /// <param name="state">The other State to check.</param>
        /// <returns>Whether this State is equals to the given one.</returns>
        public bool Equals(AbstractState state) => GetInstanceID() == state.GetInstanceID();

        /// <summary>
        /// Waits until this State is executing.
        /// </summary>
        /// <returns></returns>
        public IEnumerator WaitWhileIsExecuting() => new WaitWhile(() => IsExecuting);

        internal void ActiveState()
        {
            IsExecuting = true;

            ResetTime();
            EnterState();
        }

        internal void InternalUpdateState()
        {
            TotalFrames++;
            TotalSeconds += Time.deltaTime;
            UpdateState();
        }

        internal void DesactiveState()
        {
            IsExecuting = false;

            ResetTime();
            ExitState();
        }

        protected virtual void EnterState() => OnEnter?.Invoke();
        protected virtual void UpdateState() => OnUpdate?.Invoke();
        protected virtual void ExitState() => OnExit?.Invoke();

        private void ResetTime()
        {
            TotalFrames = 0;
            TotalSeconds = 0F;
        }

        private void CheckStateMachine()
        {
            var hasStateMachine = transform.GetComponentInParent<AnimatorStateMachine>() != null;
            if (hasStateMachine) return;

            var root = transform.root.gameObject;
            var msg = $"Required {nameof(AnimatorStateMachine)} component was attached into {root.name} GameObject.";

            root.AddComponent<AnimatorStateMachine>();

            Debug.LogWarning(msg);
        }

        protected static int GetFrames() => Time.frameCount;
        protected static float GetTime() => Time.timeSinceLevelLoad;
    }
}