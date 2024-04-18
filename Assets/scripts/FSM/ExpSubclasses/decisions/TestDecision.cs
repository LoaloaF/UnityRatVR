using FSM;
using UnityEngine;
namespace ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Decisions/TestDecision")]
    public class TestDecision : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            Debug.Log("TestDecision");
            return true;
        }
    }
}