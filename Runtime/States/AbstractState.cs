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
        public event Action OnEntered;
        public event Action OnUpdated;
        public event Action OnExited;

        public AnimatorStateMachine StateMachine { get; internal set; }

        public bool Enabled => gameObject.activeInHierarchy && enabled;
        public bool IsExecuting { get; private set; }

        public ulong TotalFrames => StateMachine.TotalFrames;
        public float TotalSeconds => StateMachine.TotalSeconds;

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
        /// Waits while this State is executing.
        /// </summary>
        /// <returns></returns>
        public IEnumerator WaitWhileIsExecuting() => new WaitWhile(() => IsExecuting);

        /// <summary>
        /// Asynchronously waits while this State is executing.
        /// </summary>
        /// <returns></returns>
        public async Awaitable WaitWhileIsExecutingAsync()
        {
            while (IsExecuting)
            {
                await Awaitable.NextFrameAsync(destroyCancellationToken);
            }
        }

        internal void ExecuteEnterState()
        {
            IsExecuting = true;
            EnterState();
            OnEntered?.Invoke();
        }

        internal void ExecuteUpdateState()
        {
            UpdateState();
            OnUpdated?.Invoke();
        }

        internal void ExecuteExitState()
        {
            IsExecuting = false;
            ExitState();
            OnExited?.Invoke();
        }

        protected virtual void EnterState() { }
        protected virtual void UpdateState() { }
        protected virtual void ExitState() { }

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