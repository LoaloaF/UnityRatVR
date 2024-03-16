using FSM;
using UnityEngine;
using UnityEngine.AI;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/Initilization")]
    public class Initilization : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            Debug.Log("Initilization (Action) would execture here and now.");
            // var pillarManager = stateMachine.GetComponent<PillarManager>();
            // pillarManager.CreateNewCheckpoint();

        }
    }
}