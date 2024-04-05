using FSM;
using System;
using System.Diagnostics;
using UnityEngine;


namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Actions/ActionAtInit")]
    public class ActionAtInit : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            UnityEngine.Debug.Log("Runs when in init-state"); 
            // var pillarManager = stateMachine.GetComponent<PillarManager>();
            if (stateMachine.scene != null)
            {
                UnityEngine.Debug.Log("state machine scene is not null");
                // var pillardata = stateMachine.scene.Pillars;
                // Vector3 pos = pillardata[childcount - 1].Position;
                // Debug.Log("name from scene object data: " + pillardata[childcount - 1].UID);
            }
            else
            {
                throw new Exception("state machine scene is null");
                // Debug.Log("state machine scene is null");
            }

            // pillarManager.CreateNewCheckpoint();

        }
    }
}
