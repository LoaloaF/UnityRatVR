using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0500/TrialInitMotorLearning")]

    public class TrialInitMotorLearning : FSMAction
    {
        public Queue<float> rawMovementQueue;
        public Queue<float> yawMovementQueue;
        public Queue<float> pitchMovementQueue;

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

            if (stateMachine.transform.childCount > 0)
            {
                Transform child = stateMachine.transform.GetChild(0);
                child.gameObject.SetActive(false);
            }
            // Transform child = stateMachine.transform.GetChild(0);
            // child.gameObject.SetActive(false);

            stateMachine._playerMovement.DisableMovement();
            stateMachine._playerMovement.gain = new Vector3(1f, 1f, 1f);


            rawMovementQueue.Enqueue(stateMachine._playerMovement.XYZvelInput[0] * 
                                     stateMachine._playerMovement.ballForwardNormToCentimeter);
            yawMovementQueue.Enqueue(stateMachine._playerMovement.XYZvelInput[1] *
                                     stateMachine._playerMovement.ballSidewaysNormToCentimeter);
            pitchMovementQueue.Enqueue(stateMachine._playerMovement.XYZvelInput[2] *
                                       stateMachine._playerMovement.ballRotatationNormToCentimeter);
        }

        private void OnEnable() 
        {
            rawMovementQueue = new Queue<float>();
            yawMovementQueue = new Queue<float>();
            pitchMovementQueue = new Queue<float>();
        }

    }
}
