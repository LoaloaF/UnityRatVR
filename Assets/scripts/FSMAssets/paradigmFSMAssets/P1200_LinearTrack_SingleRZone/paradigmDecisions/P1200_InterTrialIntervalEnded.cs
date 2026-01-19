using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Decisions/P1100/P1100_InterTrialIntervalEnded")]
    public class P1100_InterTrialIntervalEnded: Decision
    {
        public float timer = 0f;
        private int currentTrialID = -1;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            if (currentTrialID != stateMachine._sessionManager._currentTrialID)
            {
                currentTrialID = stateMachine._sessionManager._currentTrialID;
                timer = 0f;
            }
            
            int trialRewardNum = stateMachine._sessionManager.currentRewardNum;

            if (trialRewardNum > 0 && timer > stateMachine._sessionManager.interTrialIntervalLength) {
                return true;
            }
            else if (trialRewardNum <= 0 && timer > stateMachine._sessionManager.abortInterTrialIntervalLength) {
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


