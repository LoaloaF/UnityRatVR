using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Transition")]
    public sealed class Transition : ScriptableObject
    {
        public Decision Decision;
        public BaseState TrueState;
        public BaseState FalseState;
        private int tmpStateID;

        public void Execute(BaseStateMachine stateMachine)
        {
            if (Decision == null)
            {
                stateMachine.CurrentState = TrueState;
            }
            else if(Decision.Decide(stateMachine) && !(TrueState is RemainInState)) {
                int paradigmID = stateMachine.generalCurrentStateID/100;
                stateMachine.generalCurrentStateID = TrueState.stateID+paradigmID*100;
                
                stateMachine.CurrentState = TrueState;
            }
            
            else if(!(FalseState is RemainInState))
                stateMachine.CurrentState = FalseState;
        }
    }
}
