using FSM;
using System;
using System.Diagnostics;
using UnityEngine;
using RatVR.Scene;


namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Actions/General/SessionEndedAction")]
    public class SessionEndedAction : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            stateMachine.validationSphereRenderer.enabled = true;
            stateMachine.validationSphereRenderer.material.color = Color.black;

            stateMachine._sceneController.wallTop.SetActive(false);
            stateMachine._sceneController.wallBottom.SetActive(false);
            stateMachine._sceneController.wallLeft.SetActive(false);
            stateMachine._sceneController.wallRight.SetActive(false);

            stateMachine._sceneController.floor.SetActive(false);
            stateMachine._sceneController.wallZone.SetActive(false);
            stateMachine._playerMovement.DisableMovement();
            stateMachine._playerMovement.gain = new Vector3(1f, 1f, 1f);
            stateMachine._sessionManager.trialRunning = false;
            stateMachine._sessionManager.sessionRunning = false;


            for (int i = stateMachine.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = stateMachine.transform.GetChild(i);
                Destroy(child.gameObject);
            }

        }
 
    }
}
