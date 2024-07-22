using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Transition")]
    public sealed class Transition : ScriptableObject
    {
        public Decision Decision;
        public BaseState TrueState;
        public BaseState FalseState;

        public void Execute(BaseStateMachine stateMachine)
        {
            if (Decision == null)
            {
                stateMachine.CurrentState = TrueState;
            }
            else if(Decision.Decide(stateMachine) && !(TrueState is RemainInState)) {
                Debug.Log($"Transition from {stateMachine.CurrentState} to {TrueState}");
                stateMachine.CurrentState = TrueState;
            }
            else if(!Decision.Decide(stateMachine) && !(FalseState is RemainInState)){
                Debug.Log($"Transition from {stateMachine.CurrentState} to {FalseState}");
                stateMachine.CurrentState = FalseState;
            }
        }
    }
}
