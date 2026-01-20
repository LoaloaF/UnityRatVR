using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1200/P1200_ResetCurrentRewardInDR")]

    public class P1200_ResetCurrentRewardInDR : FSMAction
    {
        public P0800_TrialStartLinearTrack trialStartLinearTrack;
        private int currentTrialID = -1;
        private bool needReset = false;
        public override void Execute(BaseStateMachine stateMachine)
        {
            if (currentTrialID != stateMachine._sessionManager._currentTrialID)
            {
                currentTrialID = stateMachine._sessionManager._currentTrialID;
                needReset = true;
            }

            int doubleReward = int.Parse(stateMachine._sessionManager.trialVariablesDict["DR"]);
            int currentRewardNum = stateMachine._sessionManager.currentRewardNum;
            if (doubleReward == 1 && needReset) 
            {
                if (stateMachine._sessionManager.rewardSucked)
                {
                    trialStartLinearTrack.firstRewardNum = currentRewardNum - 1;
                    stateMachine._sessionManager.rewardSucked = false;
                }
                else
                {
                    trialStartLinearTrack.firstRewardNum = currentRewardNum;
                }

                stateMachine._sessionManager.currentRewardNum = 0;
                Debug.Log("first Reward Number: " + trialStartLinearTrack.firstRewardNum);
                needReset = false;
            }

        }
    }
}
