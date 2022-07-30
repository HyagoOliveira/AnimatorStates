using System;
using UnityEngine;

namespace ActionCode.AnimatorStates
{
    /// <summary>
    /// Abstract component used by <see cref="MonoBehaviourState"/>.
    /// </summary>
    public abstract class AbstractMonoBehaviourState : MonoBehaviour
    {
        /// <summary>
        /// Event fired when entering this State.
        /// </summary>
        public event Action OnEnter;

        /// <summary>
        /// Event fired when updating this State.
        /// </summary>
        public event Action OnUpdate;

        /// <summary>
        /// Event fired when exiting this State.
        /// </summary>
        public event Action OnExit;

        /// <summary>
        /// Whether this State is currently enabled.
        /// </summary>
        public bool Enabled => gameObject.activeInHierarchy && enabled;

        /// <summary>
        /// The total frames in this State since it was activated from the last time.
        /// </summary>
        public ulong TotalFrames { get; private set; }

        /// <summary>
        /// The total seconds in this State since it was activated from the last time.
        /// </summary>
        public float TotalSeconds { get; private set; }

        /// <summary>
        /// Whether this State is executing.
        /// </summary>
        public bool IsExecuting { get; private set; }

        public override string ToString() => GetType().Name;

        internal void StateEnter()
        {
            IsExecuting = true;
            ResetTime();
            EnterState();
        }

        internal void StateUpdate()
        {
            TotalFrames++;
            TotalSeconds += Time.deltaTime;
            UpdateState();
        }

        internal void StateExit()
        {
            ResetTime();
            ExitState();
            IsExecuting = false;
        }

        protected virtual void EnterState() => OnEnter?.Invoke();
        protected virtual void UpdateState() => OnUpdate?.Invoke();
        protected virtual void ExitState() => OnExit?.Invoke();

        protected static float GetTime() => Time.timeSinceLevelLoad;

        private void ResetTime()
        {
            TotalFrames = 0;
            TotalSeconds = 0F;
        }
    }
}