using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0900/P0900_RewardConditionReached")]
    public class P0900_RewardConditionReached : Decision
    {
        public P0500_MovementInQueue movementInQueue;
        public P0500_TrialInitMotorLearning trialInitMotorLearning;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            return LickReward(stateMachine);
        }

        private bool LickReward(BaseStateMachine stateMachine)
        {
            var portentaPackage = trialInitMotorLearning.portentaOutputSHMInterface.PopExtractedItem();
            if (portentaPackage != null && portentaPackage["N"].ToString().Trim() == "L") 
            {
                stateMachine._sessionManager.rewardPresent = false;
                return true;
            }
            else 
                return false;
        }
    }

}


