using System;
using System.Linq;
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

        /// <summary>
        /// The current number of layers.
        /// </summary>
        public int LayersCount => animator.layerCount;

        internal ulong TotalFrames { get; private set; }
        internal float TotalSeconds { get; private set; }

        private Dictionary<Type, AbstractState> states;
        private Dictionary<int, AnimatorStateMachineLayer> layers;

        private void Reset() => animator = GetComponent<Animator>();

        private void Awake()
        {
            InitializeStates();
            InitializeLayers();
        }

        // StateMachineBehaviour lost references every time the Animator is disabled.
        private void OnEnable() => ResetBinders();

        /// <summary>
        /// Resets all State Binders.
        /// </summary>
        public void ResetBinders() => InitializeBinders();

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

        #region Get Last/Current State
        /// <summary>
        /// Returns the last state from the given index.
        /// </summary>
        /// <param name="layer">The layer index.</param>
        /// <returns>A <see cref="AbstractState"/> instance or null.</returns>
        public AbstractState GetLastState(int layer = 0) => GetLayer(layer).LastState;

        /// <summary>
        /// Returns the current state from the given index.
        /// </summary>
        /// <param name="layer"><inheritdoc cref="GetLastState(int)"/></param>
        /// <returns><inheritdoc cref="GetLastState(int)"/></returns>
        public AbstractState GetCurrentState(int layer = 0) => GetLayer(layer).CurrentState;

        /// <summary>
        /// Returns the last states from all layers.
        /// </summary>
        /// <returns>An array of <see cref="AbstractState"/>.</returns>
        public AbstractState[] GetLastStates()
        {
            return layers.Values
                .Where(l => l.LastState != null)
                .Select(l => l.LastState)
                .ToArray();
        }

        /// <summary>
        /// Returns the current states from all layers.
        /// </summary>
        /// <returns><inheritdoc cref="GetLastStates()"/></returns>
        public AbstractState[] GetCurrentStates()
        {
            return layers.Values
                .Where(l => l.CurrentState != null)
                .Select(l => l.CurrentState)
                .ToArray();
        }
        #endregion

        #region Was/Is Executing
        /// <summary>
        /// Returns whether it is executing the given State type.
        /// </summary>
        /// <typeparam name="T">The State type.</typeparam>
        /// <returns>Whether it is executing the given State type.</returns>
        public bool IsExecuting<T>() where T : IState => GetState(typeof(T)).IsExecuting;

        /// <summary>
        /// Returns whether it is executing the given State type.
        /// </summary>
        /// <typeparam name="T">The State type.</typeparam>
        /// <returns>Whether it is executing the given State type.</returns>
        public bool WasExecuting<T>() where T : IState
        {
            foreach (var layer in layers.Values)
            {
                var wasExecuting =
                    layer.LastState is T &&
                    !layer.LastState.IsExecuting;

                if (wasExecuting) return true;
            }
            return false;
        }
        #endregion

        #region Is Animator State
        /// <summary>
        /// Whether the given name matches the current Animator state.
        /// </summary>
        /// <param name="name">Name of the state.</param>
        /// <param name="layer">The layer index.</param>
        /// <returns>True if matches. False otherwise.</returns>
        public bool IsAnimatorState(string name, int layer = 0) =>
            animator.GetCurrentAnimatorStateInfo(layer).IsName(name);

        /// <summary>
        /// Whether the given id matches the current Animator state.
        /// </summary>
        /// <param name="id">The state ID. Use <see cref="Animator.StringToHash(string)"/> to hash it.</param>
        /// <param name="layer">The layer index.</param>
        /// <returns>True if matches. False otherwise.</returns>
        public bool IsAnimatorState(int id, int layer = 0) =>
            animator.GetCurrentAnimatorStateInfo(layer).shortNameHash == id;
        #endregion

        /// <summary>
        /// Returns the layer using th given index.
        /// </summary>
        /// <param name="index">The layer index.</param>
        /// <returns>A <see cref="AnimatorStateMachineLayer"/> or null.</returns>
        public AnimatorStateMachineLayer GetLayer(int index) =>
            layers.ContainsKey(index) ? layers[index] : null;

        internal void EnterState(AbstractState state, int layerIndex)
        {
            ResetTime();
            layers[layerIndex].EnterState(state);
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
            layers[layerIndex].ExitState(state);
            state.ExecuteExitState();
        }

        private void InitializeStates()
        {
            var childrenStates = GetComponentsInChildren<AbstractState>(includeInactive: true);
            states = new(childrenStates.Length);
            foreach (var state in childrenStates)
            {
                var key = state.GetType();

                state.StateMachine = this;
                states.Add(key, state);
            }
        }

        private void InitializeBinders()
        {
            // animator.GetBehaviours() can only be called after Awake function.
            var binders = animator.GetBehaviours<AbstractBinder>();
            foreach (var binder in binders)
            {
                binder.Initialize(this);
            }
        }

        private void InitializeLayers()
        {
            layers = new(LayersCount);
            for (int i = 0; i < LayersCount; i++)
            {
                var layer = new AnimatorStateMachineLayer(i, animator);
                layers.Add(i, layer);
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