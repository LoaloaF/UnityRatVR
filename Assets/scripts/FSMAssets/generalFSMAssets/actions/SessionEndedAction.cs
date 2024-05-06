using FSM;
using System;
using System.Diagnostics;
using UnityEngine;
using RatVR.Scene;


namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Actions/SessionEndedAction")]
    public class SessionEndedAction : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            stateMachine.validationSphereRenderer.enabled = true;
            stateMachine.validationSphereRenderer.material.color = Color.black;
            stateMachine._sceneController.floor.SetActive(false);
            stateMachine._sceneController.wallzone.SetActive(false);
            stateMachine._playerMovement.DisableMovement();
            stateMachine._sessionManager.trialRunning = false;
        }
 
    }
}
