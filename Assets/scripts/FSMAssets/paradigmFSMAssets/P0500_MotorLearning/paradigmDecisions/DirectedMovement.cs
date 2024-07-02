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
        public TrialInitMotorLearning trialInitMotorLearning;
        [SerializeField] private bool returnTrue;

        public override bool Decide(BaseStateMachine stateMachine)
        {
            float moveThreshold = float.Parse(stateMachine._sessionManager.trialVariablesDict["MTH"]);

            float rawMovementSum = CalculateQueueSum(trialInitMotorLearning.rawMovementQueue);
            float yawMovementSum = CalculateQueueSum(trialInitMotorLearning.yawMovementQueue);
            float pitchMovementSum = CalculateQueueSum(trialInitMotorLearning.pitchMovementQueue);
            float movementSum = rawMovementSum + yawMovementSum + pitchMovementSum;
            Debug.Log($"rawMovementSum: {rawMovementSum/movementSum}, yawMovementSum: {yawMovementSum/movementSum}, pitchMovementSum: {pitchMovementSum/movementSum}");

            if (rawMovementSum/movementSum > moveThreshold || 
                yawMovementSum/movementSum > moveThreshold ||
                pitchMovementSum/movementSum > moveThreshold)
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


