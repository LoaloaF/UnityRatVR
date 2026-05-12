using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P1300/P1300_RewardConditionReached")]
    public class P1300_RewardConditionReached : Decision
    {
        private float rawMovementTemp;
        private float yawMovementTemp;
        private float pitchMovementTemp;
        private float movementSummation;
        private float stopThreshold;
        public P1300_TrialInitLinearTrack trialInitLinearTrack;
        public P1300_TrialStartLinearTrack trialStartLinearTrack;
        public P1300_MovementInitiation movementInitiation;

        public override bool Decide(BaseStateMachine stateMachine)
        {
            return StopMovement(stateMachine);
        }

        private bool StopMovement(BaseStateMachine stateMachine)
        {
            Debug.Log("movement initiation forward queue: " + string.Join(", ", movementInitiation.rawMovementQueue));
            float forwardSum = Mathf.Abs(CalculateQueueSum(movementInitiation.rawMovementQueue));
            float lateralSum = Mathf.Abs(CalculateQueueSum(movementInitiation.yawMovementQueue))
                             + Mathf.Abs(CalculateQueueSum(movementInitiation.pitchMovementQueue));

            float forwardThreshold = float.Parse(stateMachine._sessionManager.trialVariablesDict["ST_F"]);
            float lateralThreshold = float.Parse(stateMachine._sessionManager.trialVariablesDict["ST_L"]);

            return forwardSum < forwardThreshold && lateralSum < lateralThreshold;
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


