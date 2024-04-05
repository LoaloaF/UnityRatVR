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

            if (stateMachine.scene == null){
                throw new Exception("state machine scene is null");
                // UnityEngine.Debug.Log("adding state machine scene in ActionAtInit");
                //stateMachine.scene = stateMachine.Getcomponent<SceneController>().scene;

                // var s = stateMachine.Getcomponent<Transform>().SceneController;
            }

            // pillarManager.CreateNewCheckpoint();

        }
    }
}
