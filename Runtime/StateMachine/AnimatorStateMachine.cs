using System;
using System.Collections.Generic;
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

        /// <summary>
        /// The current number of States.
        /// </summary>
        public int Size => states.Count;

        public Dictionary<int, AbstractState> LastStates { get; private set; }
        public Dictionary<int, AbstractState> CurrentStates { get; private set; }

        internal ulong TotalFrames { get; private set; }
        internal float TotalSeconds { get; private set; }

        private readonly Dictionary<Type, AbstractState> states = new();

        private void Reset() => animator = GetComponent<Animator>();

        private void Awake()
        {
            InitializeStates();
            InitializeBinders();

            // Animators may have multiple last/current states since they have multiples layers.
            var layers = animator.layerCount;
            LastStates = new(layers);
            CurrentStates = new(layers);

            for (int i = 0; i < layers; i++)
            {
                LastStates.Add(i, null);
                CurrentStates.Add(i, null);
            }
        }

        #region Has State
        /// <summary>
        /// Returns whether it has the given State.
        /// </summary>
        /// <typeparam name="T">The State type.</typeparam>
        /// <returns>Whether it has the given State.</returns>
        public bool HasState<T>() where T : IState => states.ContainsKey(typeof(T));

        /// <summary>
        /// Returns whether it has the given State type.
        /// </summary>
        /// <param name="stateType">The State type.</param>
        /// <returns>Whether it has the given State type.</returns>
        public bool HasState(Type stateType) => states.ContainsKey(stateType);

        /// <summary>
        /// Returns whether it has the given State instance.
        /// </summary>
        /// <param name="state">The State instance.</param>
        /// <returns>Whether it has the given State.</returns>
        public bool HasState(AbstractState state) => states.ContainsValue(state);

        /// <summary>
        /// Returns whether it has the given State by using its name.
        /// </summary>
        /// <param name="stateName">The State name.</param>
        /// <returns>Whether it has the given State.</returns>
        public bool HasState(string stateName)
        {
            var state = GetState(stateName);
            return state != null;
        }
        #endregion

        #region Get State
        /// <summary>
        /// Gets the given State only if it is present.
        /// </summary>
        /// <typeparam name="T">The State type.</typeparam>
        /// <returns>The State component instance or null if not present.</returns>
        public T GetState<T>() where T : AbstractState => HasState<T>() ? (T)states[typeof(T)] : null;

        /// <summary>
        /// Gets the given State only if it is present.
        /// </summary>
        /// <param name="stateType">The State type.</param>
        /// <returns>The State component instance or null if not present.</returns>
        public AbstractState GetState(Type stateType) => HasState(stateType) ? states[stateType] : null;

        /// <summary>
        /// Gets the given State only if it is present.
        /// </summary>
        /// <param name="stateName">The State name.</param>
        /// <returns>The State component instance or null if not present.</returns>
        public AbstractState GetState(string stateName)
        {
            foreach (var type in states.Keys)
            {
                var isState = type.Name.Equals(stateName, StringComparison.OrdinalIgnoreCase);
                if (isState) return states[type];
            }
            return null;
        }

        /// <summary>
        /// Gets all States as the given type.
        /// </summary>
        /// <typeparam name="T">The State type</typeparam>
        /// <returns>The State component instances as an array.</returns>
        public T[] GetStates<T>() where T : AbstractState
        {
            T[] arrayState = new T[Size];
            states.Values.CopyTo(arrayState, 0);
            return arrayState;
        }
        #endregion

        #region Is Executing
        /// <summary>
        /// Returns whether it is executing the given State.
        /// </summary>
        /// <typeparam name="T">The State type.</typeparam>
        /// <returns>Whether it is executing the given State.</returns>
        public bool IsExecuting<T>() where T : IState
        {
            foreach (var state in CurrentStates.Values)
            {
                if (state is T) return true;
            }
            return false;
        }

        /// <summary>
        /// Returns whether it is executing the given State instance.
        /// </summary>
        /// <param name="state">The State instance.</param>
        /// <returns>Whether it is executing the given State.</returns>
        public bool IsExecuting(AbstractState state) => CurrentStates.ContainsValue(state);
        #endregion

        #region Was Executing
        /// <summary>
        /// Returns whether it was executing the given State.
        /// </summary>
        /// <typeparam name="T">The State type.</typeparam>
        /// <returns>Whether it was executing the given State.</returns>
        public bool WasExecuting<T>() where T : IState
        {
            foreach (var state in LastStates.Values)
            {
                if (state is T) return true;
            }
            return false;
        }

        /// <summary>
        /// Returns whether it was executing the given State instance.
        /// </summary>
        /// <param name="state">The State instance.</param>
        /// <returns>Whether it was executing the given State.</returns>
        public bool WasExecuting(AbstractState state) => LastStates.ContainsValue(state);
        #endregion

        internal void EnterState(AbstractState state, int layerIndex)
        {
            ResetTime();
            CurrentStates[layerIndex] = state;
            state.ExecuteEnterState();
        }

        internal void UpdateState(AbstractState state)
        {
            UpdateTime();
            state.ExecuteUpdateState();
        }

        internal void ExitState(AbstractState state, int layerIndex)
        {
            ResetTime();
            LastStates[layerIndex] = state;
            state.ExecuteExitState();
        }

        private void InitializeStates()
        {
            var childrenStates = GetComponentsInChildren<AbstractState>(includeInactive: true);
            foreach (var state in childrenStates)
            {
                var key = state.GetType();

                state.StateMachine = this;
                states.Add(key, state);
            }
        }

        private void InitializeBinders()
        {
            var binders = animator.GetBehaviours<AbstractBinder>();
            foreach (var binder in binders)
            {
                binder.Initialize(this);
            }
        }

        private void ResetTime()
        {
            TotalFrames = 0;
            TotalSeconds = 0F;
        }

        private void UpdateTime()
        {
            TotalFrames++;
            TotalSeconds += Time.deltaTime;
        }
    }
}