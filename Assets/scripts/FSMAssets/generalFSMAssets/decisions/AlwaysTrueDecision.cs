using FSM;
using UnityEngine;
namespace ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Decisions/General/AlwaysTrueDecision")]
    public class AlwaysTrueDecision : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            return true;
        }
    }
}