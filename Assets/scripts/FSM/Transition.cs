using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Transition")]
    public sealed class Transition : ScriptableObject
    {
        public Decision Decision;
        public BaseState TrueState;
        public BaseState FalseState;
        private bool _isTrueState;

        public void Execute(BaseStateMachine stateMachine)
        {
            if (Decision == null)
            {
                stateMachine.CurrentState = TrueState;
            }
            else
            {
                _isTrueState = Decision.Decide(stateMachine);
                if (_isTrueState && !(TrueState is RemainInState))
                {
                    Debug.Log($"Transition from {stateMachine.CurrentState} to {TrueState}");
                    stateMachine.CurrentState = TrueState;
                }
                else if (!_isTrueState && !(FalseState is RemainInState))
                {
                    Debug.Log($"Transition from {stateMachine.CurrentState} to {FalseState}");
                    stateMachine.CurrentState = FalseState;
                }
            }
        }
    }
}
