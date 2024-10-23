using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P1000/P1000_RewardConditionReached")]
    public class P1000_RewardConditionReached : Decision
    {
        public P0500_MovementInQueue movementInQueue;
        public P0500_TrialInitMotorLearning trialInitMotorLearning;
        public float timer = 0f;
        private int currentTrialID = -1;

        public override bool Decide(BaseStateMachine stateMachine)
        {
            return LickReward(stateMachine) && TimeReached(stateMachine);
        }

        private bool LickReward(BaseStateMachine stateMachine)
        {
            var portentaPackage = trialInitMotorLearning.portentaOutputSHMInterface.PopExtractedItem();
            if (portentaPackage != null && portentaPackage["N"].ToString().Trim() == "L") 
                return true;
            else 
                return false;
        }



        private bool TimeReached(BaseStateMachine stateMachine)
        {
            float stopTime = float.Parse(stateMachine._sessionManager.trialVariablesDict["ST"]);

            if (currentTrialID != stateMachine._sessionManager._currentTrialID)
            {
                currentTrialID = stateMachine._sessionManager._currentTrialID;
                timer = 0f;
            }
            
            if (timer > stopTime) {
                return true;
            }
            
            timer += Time.deltaTime;
            return false;
        }

        private void OnEnable() 
        {
            timer = 0;
            currentTrialID = -1;
        }

    }

}


