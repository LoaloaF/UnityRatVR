using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0500/DirectedMovement")]
    public class DirectedMovement : Decision
    {
        public TrialStartMotorLearning trialStartMotorLearning;
        public bool returnTrue;

        private float rawMovementSum;
        private float yawMovementSum;
        private float pitchMovementSum;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            float moveThreshold = float.Parse(stateMachine._sessionManager.trialVariablesDict["MTH"]);

            int checkRaw = int.Parse(stateMachine._sessionManager.trialVariablesDict["R"]);
            int checkYaw = int.Parse(stateMachine._sessionManager.trialVariablesDict["Y"]);
            int checkPitch = int.Parse(stateMachine._sessionManager.trialVariablesDict["P"]);

            rawMovementSum = Mathf.Abs(CalculateQueueSum(trialStartMotorLearning.rawMovementQueue));
            yawMovementSum = Mathf.Abs(CalculateQueueSum(trialStartMotorLearning.yawMovementQueue));
            pitchMovementSum = Mathf.Abs(CalculateQueueSum(trialStartMotorLearning.pitchMovementQueue));

            float movementSum = rawMovementSum + yawMovementSum + pitchMovementSum;
            Debug.Log($"rawMovementSum: {rawMovementSum}, yawMovementSum: {yawMovementSum}, pitchMovementSum: {pitchMovementSum}");


            if ((checkRaw == 1 && rawMovementSum/movementSum > moveThreshold) || 
                (checkYaw == 1 && yawMovementSum/movementSum > moveThreshold) ||
                (checkPitch == 1 && pitchMovementSum/movementSum > moveThreshold))
            {
                return returnTrue;
            }
            else
            {
                return !returnTrue;
            }

        }

        private float CalculateQueueSum(Queue<float> queue)
        {
            float sum = 0;
            foreach (float item in queue)
            {
                sum += item;
            }
            return sum;
        }

    }

}


