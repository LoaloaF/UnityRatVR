using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1100/P1100_ResetCurrentRewardInDR")]

    public class P1100_ResetCurrentRewardInDR : FSMAction
    {
        public P0800_TrialStartLinearTrack trialStartLinearTrack;
        public override void Execute(BaseStateMachine stateMachine)
        {
            int doubleReward = int.Parse(stateMachine._sessionManager.trialVariablesDict["DR"]);
            int currentRewardNum = stateMachine._sessionManager.currentRewardNum;
            if (doubleReward == 1)
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
            }

        }
    }
}
