using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

namespace ActionCode.AnimatorStates
{
    /// <summary>
    /// GUI debugger inspector for Animator states.
    /// <para>Use it to show the Current and Last State from an Animator.</para>
    /// <para><b>This component only works on Editor Mode.</b></para>
    /// </summary>
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public sealed class AnimatorStateMachineGUI : MonoBehaviour
    {
        [SerializeField, Tooltip("The local Animator component.")]
        private Animator animator;
        public string title = "States";
        public Rect area = new(60f, 30f, 180f, 64f);

        /// <summary>
        /// All states available in the <see cref="animator"/>.
        /// </summary>
        public Dictionary<int, string> States { get; private set; }

        public string LastStateName { get; private set; } = NULL_STATE_NAME;
        public string CurrentStateName { get; private set; } = NULL_STATE_NAME;

        private int lines;
        private int frames = 0;
        private GUIStyle style;

        private const int LAYER = 0;
        private const string NULL_STATE_NAME = "NONE";

        private void Reset() => animator = GetComponent<Animator>();

        private void Start()
        {
            SetupStyle();
            LoadAnimatorStates();
        }

        private void Update()
        {
            if (Application.isPlaying) frames++;
        }

        private void OnGUI()
        {
            lines = 1;

            var stateInfo = GetCurrentAnimatorStateInfo();
            var hash = stateInfo.shortNameHash;
            var hasState = States?.ContainsKey(hash) == true;
            var newStateName = hasState ? States[hash] : NULL_STATE_NAME;
            var differentState = !CurrentStateName.Equals(newStateName);

            if (differentState)
            {
                frames = 0;
                LastStateName = CurrentStateName;
            }

            CurrentStateName = newStateName;

            GUI.BeginGroup(area, title, new GUIStyle("Box"));
#if UNITY_EDITOR
            DrawField("Last", LastStateName);
            DrawField("Current", CurrentStateName + " F: " + frames);
#else
            DrawLine("Cannot read Animator States out of Editor Mode.");
#endif
            GUI.EndGroup();
        }

        private void DrawField(string label, string value) => DrawLine($"{label}: {value}");

        private void DrawLine(string value)
        {
            var size = new Vector2(area.width, GetLineHeight());
            var position = new Vector2(8F, 8F + lines * GetLineHeight());

            GUI.Label(new Rect(position, size), value, style);
            lines++;
        }

        void LoadAnimatorStates()
        {
            if (animator.runtimeAnimatorController == null) return;
#if UNITY_EDITOR
            var path = AssetDatabase.GetAssetPath(animator.runtimeAnimatorController);
            var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
            var rootStateMachine = controller.layers[LAYER].stateMachine;

            States = new Dictionary<int, string>(rootStateMachine.states.Length);

            foreach (var childState in rootStateMachine.states)
            {
                var state = childState.state;
                States.Add(state.nameHash, state.name);
            }
#endif
        }

        private float GetLineHeight() => style.lineHeight;

        private void SetupStyle()
        {
            style = GUIStyle.none;
            style.normal.textColor = Color.white;
        }

        private AnimatorStateInfo GetCurrentAnimatorStateInfo()
        {
            // Necessary to don't display Warning Messages on Console.
            var isValidAnimator = animator && Application.isPlaying;
            return isValidAnimator ?
                animator.GetCurrentAnimatorStateInfo(LAYER) :
                new AnimatorStateInfo();
        }
    }
}