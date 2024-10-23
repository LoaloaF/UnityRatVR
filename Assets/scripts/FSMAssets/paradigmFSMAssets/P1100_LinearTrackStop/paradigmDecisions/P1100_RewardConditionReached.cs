using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P1100/P1100_RewardConditionReached")]
    public class P1100_RewardConditionReached : Decision
    {
        private float rawMovementTemp;
        private float yawMovementTemp;
        private float pitchMovementTemp;
        private float movementSummation;
        public P0800_TrialInitLinearTrack trialInitLinearTrack;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            int currentRewardNum = int.Parse(stateMachine._sessionManager.trialVariablesDict["RN"]);

            if (currentRewardNum == 0)
            {
                return StopMovement(stateMachine);
            }
            else
                return StopMovement(stateMachine) && LickReward(stateMachine);

        }

        private bool LickReward(BaseStateMachine stateMachine)
        {
            var portentaPackage = trialInitLinearTrack.portentaOutputSHMInterface.PopExtractedItem();
            if (portentaPackage != null && portentaPackage["N"].ToString().Trim() == "L") 
                return true;
            else 
                return false;
        }


        private bool StopMovement(BaseStateMachine stateMachine)
        {
            float stopThreshold = float.Parse(stateMachine._sessionManager.trialVariablesDict["ST"]);
            
            rawMovementTemp = stateMachine._playerMovement.XYZvelInput[0] * stateMachine._playerMovement.ballForwardNormToCentimeter;
            yawMovementTemp = stateMachine._playerMovement.XYZvelInput[1] * stateMachine._playerMovement.ballSidewaysNormToCentimeter;
            pitchMovementTemp = stateMachine._playerMovement.XYZvelInput[2] * stateMachine._playerMovement.ballRotatationNormToCentimeter;

            movementSummation = Math.Abs(rawMovementTemp) + Math.Abs(yawMovementTemp) + Math.Abs(pitchMovementTemp);

            if (movementSummation < stopThreshold)
                return true;
            else
                return false;
        
        }


    }

}


