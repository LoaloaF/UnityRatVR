using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/sessionEnded")]
    public class sessionEnded : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            if (stateMachine._sessionManager.abortTrialFlag) {
                if (stateMachine._sessionManager.trialRunning) {
                    Debug.Log("SessionEnded while trial was running");
                    stateMachine._sessionManager.logEndTrial("-1");
                    stateMachine._sessionManager.trialRunning = false;
                } else {
                    Debug.Log("SessionEnded while in ITI");
                }
                
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

                stateMachine._sessionManager.sessionRunning = false;
                return true;
            }
            return false;
        }
    }
}


