

using FSM;
using UnityEngine;


namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Actions/ActionAtInit")]
    public class ActionAtInit : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            Debug.Log("Runs when in init-state");
            // var pillarManager = stateMachine.GetComponent<PillarManager>();

            // pillarManager.CreateNewCheckpoint();

        }
    }
}
