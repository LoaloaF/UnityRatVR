using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1100/P1100_TrialEndLinearTrackStop")]

    public class P1100_TrialEndLinearTrack : FSMAction
    {
        public P0800_TrialStartLinearTrack trialStartLinearTrack;
        public override void Execute(BaseStateMachine stateMachine)
        {

            stateMachine._sessionManager.Add_Decimal(stateMachine, "ST");

            string trialPackageValuesArray = ",ST:" + stateMachine._sessionManager.trialVariablesDict["ST"] + 
                                            ",MRN:" + stateMachine._sessionManager.trialVariablesDict["MRN"] + 
                                            ",C:" + stateMachine._sessionManager.trialVariablesDict["C"] + 
                                            ",DR:"+ stateMachine._sessionManager.trialVariablesDict["DR"];

            int outcome = stateMachine._sessionManager.currentRewardNum;


            if (stateMachine._sessionManager.rewardSucked)
            {
                outcome = outcome - 1;
            }

            outcome = trialStartLinearTrack.firstRewardNum * 10 + outcome;

            stateMachine._sessionManager.logEndTrial(outcome, trialPackageValuesArray);

            stateMachine._sessionManager.trialRunning = false;
        }
    }
}
