using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0800/P0800_RewardConditionReached")]
    public class P0800_RewardConditionReached : Decision
    {
        private float rawMovementTemp;
        private float yawMovementTemp;
        private float pitchMovementTemp;
        private float movementSummation;
        public P0800_TrialInitLinearTrack trialInitLinearTrack;
        public P1100_MovementInitiation movementInitiation;
        public float timer = 0f;

        public override bool Decide(BaseStateMachine stateMachine)
        {

            if (stateMachine._sessionManager.trialVariablesDict.ContainsKey("LR"))
            {
                int lickRewardCheck = int.Parse(stateMachine._sessionManager.trialVariablesDict["LR"]);
                if (lickRewardCheck == 1)
                {
                    return LickReward(stateMachine);
                }
                else
                    return StayTimeReached(stateMachine);
            }
            else if (stateMachine._sessionManager.trialVariablesDict.ContainsKey("DR"))
            {
                int stayReward = int.Parse(stateMachine._sessionManager.trialVariablesDict["SR"]);

                if (stateMachine._sessionManager.currentRewardNum == 0)
                {
                    return StopMovement(stateMachine);
                }
                else if (stayReward == 1)
                    return LickReward(stateMachine) && StopMovement(stateMachine);
                else
                    return LickReward(stateMachine);
            }
            else
                return false;


        }

        private bool LickReward(BaseStateMachine stateMachine)
        {
            var portentaPackage = trialInitLinearTrack.portentaOutputSHMInterface.PopExtractedItem();
            if (portentaPackage != null && portentaPackage["N"].ToString().Trim() == "L") 
            {
                stateMachine._sessionManager.rewardPresent = false;
                return true;
            }
            else 
                return false;
        }


        private bool StayTimeReached(BaseStateMachine stateMachine)
        {
            float movementTime = float.Parse(stateMachine._sessionManager.trialVariablesDict["ST"]);
            
            if (timer > movementTime) {
                return true;
            }
            
            timer += Time.deltaTime;
            return false;
        }

        private void OnEnable() 
        {
            timer = 0;
        }

        // private bool StopMovement(BaseStateMachine stateMachine)
        // {
        //     float stopThreshold = float.Parse(stateMachine._sessionManager.trialVariablesDict["ST"]);
            
        //     rawMovementTemp = stateMachine._playerMovement.XYZvelInput[0] * stateMachine._playerMovement.ballForwardNormToCentimeter;
        //     yawMovementTemp = stateMachine._playerMovement.XYZvelInput[1] * stateMachine._playerMovement.ballSidewaysNormToCentimeter;
        //     pitchMovementTemp = stateMachine._playerMovement.XYZvelInput[2] * stateMachine._playerMovement.ballRotatationNormToCentimeter;

        //     movementSummation = Math.Abs(rawMovementTemp) + Math.Abs(yawMovementTemp) + Math.Abs(pitchMovementTemp);

        //     if (movementSummation < stopThreshold)
        //         return true;
        //     else
        //         return false;
        
        // }

        private bool StopMovement(BaseStateMachine stateMachine)
        {
            float rawMovementSum = Mathf.Abs(CalculateQueueSum(movementInitiation.rawMovementQueue));
            float yawMovementSum = Mathf.Abs(CalculateQueueSum(movementInitiation.yawMovementQueue));
            float pitchMovementSum = Mathf.Abs(CalculateQueueSum(movementInitiation.pitchMovementQueue));

            float movementSum = rawMovementSum + yawMovementSum + pitchMovementSum;
            float stopThreshold = float.Parse(stateMachine._sessionManager.trialVariablesDict["ST"]);
            // Debug.Log(" yaw Movement Sum: " + yawMovementSum);
            if (movementSum < stopThreshold)
                return true;
            else
                return false;
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


