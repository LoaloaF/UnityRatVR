using FSM;
using UnityEngine;


namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Decisions/RatUnderActivePillar")]
    public class RatUnderActivePillar : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            var pillarManager = stateMachine.GetComponent<PillarManager>();
            return pillarManager.CheckPlayerPillar();
        }
    }
}