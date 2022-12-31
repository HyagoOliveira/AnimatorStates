using System;

namespace ActionCode.AnimatorStates
{
    /// <summary>
    /// Interface used on objects able to be a State managed by a <see cref="AnimatorStateMachine"/>.
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Event fired when entering this State.
        /// </summary>
        event Action OnEntered;

        /// <summary>
        /// Event fired when updating this State.
        /// </summary>
        event Action OnUpdated;

        /// <summary>
        /// Event fired when exiting this State.
        /// </summary>
        event Action OnExited;

        /// <summary>
        /// Whether this State is currently enabled.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Whether this State is executing.
        /// </summary>
        bool IsExecuting { get; }

        /// <summary>
        /// The total frames in this State since it was activated from the last time.
        /// </summary>
        ulong TotalFrames { get; }

        /// <summary>
        /// The total seconds in this State since it was activated from the last time.
        /// </summary>
        float TotalSeconds { get; }

        /// <summary>
        /// The State Machine running this State.
        /// </summary>
        AnimatorStateMachine StateMachine { get; }
    }
}