using UnityEngine;

namespace ActionCode.AnimatorStates
{
    /// <summary>
    /// State Machine component for an Animator.
    /// <para>It should be placed in the same GameObject that Animator is.</para>
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public sealed class AnimatorStateMachine : MonoBehaviour
    {
        [SerializeField, Tooltip("The local Animator component.")]
        private Animator animator;

        /// <summary>
        /// All States instances available on this StateMachine.
        /// </summary>
        public AbstractAnimatorState[] States { get; private set; }

        /// <summary>
        /// The local Animator component.
        /// </summary>
        public Animator Animator => animator;

        private void Reset() => animator = GetComponent<Animator>();

        private void Start()
        {
            // States are initialized using OnEnable, which is executed before Start function.
            foreach (var state in States)
            {
                state.Start();
            }
        }

        private void OnEnable()
        {
            // States should be initialized every time an Animator is enabled 
            // since all its data are reset (flushed) to default value.
            InitializeStates();
        }

        private void Update()
        {
            foreach (var state in States)
            {
                state.Update();
            }
        }

        /// <summary>
        /// Get the first behaviour of the given type on this State Machine.
        /// </summary>
        /// <typeparam name="T">The type of the StateMachineBehaviour.</typeparam>
        /// <returns>A StateMachineBehaviour of type T if found.</returns>
        public T GetBehaviour<T>() where T : StateMachineBehaviour => Animator.GetBehaviour<T>();

        /// <summary>
        /// Does the given name matches the name of the active state in the State Machine?
        /// </summary>
        /// <param name="name">Name of the state.</param>
        /// <param name="layer">The layer index.</param>
        /// <returns>True if matches. False otherwise.</returns>
        public bool IsState(string name, int layer = 0) => Animator.GetCurrentAnimatorStateInfo(layer).IsName(name);

        /// <summary>
        /// Does the given id matches the name of the active state in the State Machine?
        /// </summary>
        /// <param name="id">The state ID. Use <see cref="Animator.StringToHash(string)"/> to hash it.</param>
        /// <param name="layer">The layer index.</param>
        /// <returns>True if matches. False otherwise.</returns>
        public bool IsState(int id, int layer = 0) => Animator.GetCurrentAnimatorStateInfo(layer).fullPathHash == id;

        /// <summary>
        /// Does any of the given ids match the name of the active state in the State Machine?
        /// </summary>
        /// <param name="layer">The layer index.</param>
        /// <param name="ids">An array of state IDs. Use <see cref="Animator.StringToHash(string)"/> to hash them.</param>
        /// <returns>True if any match. False otherwise.</returns>
        public bool IsAnyState(int layer, params int[] ids)
        {
            var stateInfo = Animator.GetCurrentAnimatorStateInfo(layer);
            foreach (var id in ids)
            {
                if (id == stateInfo.fullPathHash) return true;
            }
            return false;
        }

        #region Animator Get/Set functions
        /// <summary>
        /// <inheritdoc cref="Animator.GetBool(int)"/>
        /// </summary>
        public bool GetBool(int id) => Animator.GetBool(id);

        /// <summary>
        /// <inheritdoc cref="Animator.GetFloat(int)"/>
        /// </summary>
        public float GetFloat(int id) => Animator.GetFloat(id);

        /// <summary>
        /// <inheritdoc cref="Animator.GetInteger(int)"/>
        /// </summary>
        public int GetInteger(int id) => Animator.GetInteger(id);

        /// <summary>
        /// <inheritdoc cref="Animator.ResetTrigger(int)"/>
        /// </summary>
        public void ResetTrigger(int id) => Animator.ResetTrigger(id);

        /// <summary>
        /// <inheritdoc cref="Animator.SetTrigger(int)"/>
        /// </summary>
        public void SetTrigger(int id) => Animator.SetTrigger(id);

        /// <summary>
        /// <inheritdoc cref="Animator.SetBool(int, bool)"/>
        /// </summary>
        public void SetBool(int id, bool value) => Animator.SetBool(id, value);

        /// <summary>
        /// <inheritdoc cref="Animator.SetFloat(int, float)"/>
        /// </summary>
        public void SetFloat(int id, float value) => Animator.SetFloat(id, value);

        /// <summary>
        /// <inheritdoc cref="Animator.SetInteger(int, int)"/>
        /// </summary>
        public void SetInteger(int id, int value) => Animator.SetInteger(id, value);

        /// <summary>
        /// <inheritdoc cref="Animator.SetLayerWeight(int, float)"/>
        /// </summary>
        public void SetLayerWeight(int layerIndex, float weight) =>
            Animator.SetLayerWeight(layerIndex, weight);
        #endregion

        private void InitializeStates()
        {
            // Note: animator.GetBehaviours() can only be called after Awake function.
            States = animator.GetBehaviours<AbstractAnimatorState>();
            foreach (var state in States)
            {
                state.Initialize(this);
            }
        }
    }
}