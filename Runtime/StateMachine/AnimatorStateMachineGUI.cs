using UnityEngine;

namespace ActionCode.AnimatorStates
{
    /// <summary>
    /// GUI debugger inspector for <see cref="AnimatorStateMachine"/>.
    /// <para>Use it to show the Current and Last States.</para>
    /// </summary>
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AnimatorStateMachine))]
    public sealed class AnimatorStateMachineGUI : MonoBehaviour
    {
        [SerializeField, Tooltip("The local Animator component.")]
        private AnimatorStateMachine stateMachine;
        public string title = "States";
        public Rect area = new(60f, 30f, 180f, 64f);

        private int lines;
        private GUIStyle style;

        private const string nullStateName = "NONE";

        private void Reset() => stateMachine = GetComponent<AnimatorStateMachine>();
        private void Start() => SetupStyle();

        private void OnGUI()
        {
            lines = 1;

            var lastStateNames = nullStateName;
            var currentStateNames = nullStateName;

            if (Application.isPlaying)
            {
                lastStateNames = GetStateText(stateMachine.GetLastStates());
                currentStateNames = GetStateText(stateMachine.GetCurrentStates());
            }

            GUI.BeginGroup(area, title, new GUIStyle("Box"));

            DrawField("Last", lastStateNames);
            DrawField("Current", currentStateNames);

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

        private void SetupStyle()
        {
            style = GUIStyle.none;
            style.normal.textColor = Color.white;
        }

        private float GetLineHeight() => style.lineHeight;

        private static string GetStateText(AbstractState[] states)
        {
            return states != null && states.Length > 0 ?
                string.Join<AbstractState>(", ", states) :
                nullStateName;
        }
    }
}