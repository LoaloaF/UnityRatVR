using FSM;
using UnityEngine;
using UnityEngine.AI;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/NewPillarAction")]
    public class NewPillarAction : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            var pillarManager = stateMachine.GetComponent<PillarManager>();

            pillarManager.CreateNewCheckpoint();

        }
    }
}