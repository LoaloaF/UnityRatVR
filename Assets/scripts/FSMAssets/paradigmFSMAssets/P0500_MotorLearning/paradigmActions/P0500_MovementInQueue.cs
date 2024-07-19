using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0500/P0500_MovementInQueue")]

    public class P0500_MovementInQueue : FSMAction
    {
        public P0500_TrialStartMotorLearning trialStartMotorLearning;
        public float rawMovementTemp;
        public float yawMovementTemp;
        public float pitchMovementTemp;

        public override void Execute(BaseStateMachine stateMachine)
        {
            float moveTime = float.Parse(stateMachine._sessionManager.trialVariablesDict["MT"]);
            if (trialStartMotorLearning.rawMovementQueue.Count > moveTime * 60)
            {
                trialStartMotorLearning.rawMovementQueue.Dequeue();
                trialStartMotorLearning.yawMovementQueue.Dequeue();
                trialStartMotorLearning.pitchMovementQueue.Dequeue();
            }

            rawMovementTemp = stateMachine._playerMovement.XYZvelInput[0] * stateMachine._playerMovement.ballForwardNormToCentimeter;
            yawMovementTemp = stateMachine._playerMovement.XYZvelInput[1] * stateMachine._playerMovement.ballSidewaysNormToCentimeter;
            pitchMovementTemp = stateMachine._playerMovement.XYZvelInput[2] * stateMachine._playerMovement.ballRotatationNormToCentimeter;

            trialStartMotorLearning.rawMovementQueue.Enqueue(rawMovementTemp);
            trialStartMotorLearning.yawMovementQueue.Enqueue(yawMovementTemp);
            trialStartMotorLearning.pitchMovementQueue.Enqueue(pitchMovementTemp);
        }

    }
}
