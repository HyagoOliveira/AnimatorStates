using UnityEngine;

namespace ActionCode.AnimatorStates
{
    public class AnimatorStateMachineLayer
    {
        public int Index => index;
        public string Name => name;

        public AbstractState LastState { get; private set; }
        public AbstractState CurrentState { get; private set; }

        private readonly int index;
        private readonly string name;

        public AnimatorStateMachineLayer(int index, Animator animator)
        {
            this.index = index;
            name = animator.GetLayerName(Index);
        }

        public override string ToString() => $"{Name}. Index: {Index}";

        internal void EnterState(AbstractState state)
        {
            CurrentState = state;
        }

        internal void ExitState(AbstractState state)
        {
            LastState = state;
            CurrentState = null;
        }
    }
}