using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0500/P0500_RewardConditionReached")]
    public class P0500_RewardConditionReached : Decision
    {
        public P0500_MovementInQueue movementInQueue;
        public P0500_TrialInitMotorLearning trialInitMotorLearning;
        private int nChecks = 0;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            int lickRewardCheck = int.Parse(stateMachine._sessionManager.trialVariablesDict["LR"]);

            if (lickRewardCheck == 1)
            {
                return LickReward(stateMachine) && CheckStop(stateMachine);
            }
            else
                return CheckStop(stateMachine);
        }


        private bool CheckStop(BaseStateMachine stateMachine)
        {
            float moveThreshold = float.Parse(stateMachine._sessionManager.trialVariablesDict["STH"]);
            float movementSum = Mathf.Abs(movementInQueue.rawMovementTemp) + Mathf.Abs(movementInQueue.yawMovementTemp) + Mathf.Abs(movementInQueue.pitchMovementTemp);

            if (movementSum > moveThreshold)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool LickReward(BaseStateMachine stateMachine)
        {
            bool foundLick = false;
            // while (true) {
            //     var portentaPackage = trialInitMotorLearning.portentaOutputSHMInterface.PopExtractedItem();

            //     if (portentaPackage == null) {
            //         nChecks = 0;
            //         if (foundLick) Debug.Log($"Lick above threshold detected, after {nChecks} checks");
            //         return foundLick;
            //     }

            //     Debug.Log(portentaPackage["V"]);
            //     if (portentaPackage["N"].ToString().Trim() == "L")
            //     {
            //         // if (int.Parse(portentaPackage["V"].ToString()) > threshold) {
            //         Debug.Log($"Lick above threshold detected, after {nChecks} checks");
            //         nChecks = 0;
            //         foundLick = true;

            //         // }
            //     }
            //     nChecks++;
            // }

            var portentaPackage = trialInitMotorLearning.portentaOutputSHMInterface.PopExtractedItem();
            if (portentaPackage != null && portentaPackage["N"].ToString().Trim() == "L") 
                return true;
            else 
                return false;
        }

    }

}


