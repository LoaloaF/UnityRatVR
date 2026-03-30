using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1300/P1300_TrialEndLinearTrackStop")]

    public class P1300_TrialEndLinearTrack : FSMAction
    {
        public P1300_TrialStartLinearTrack trialStartLinearTrack;
        public override void Execute(BaseStateMachine stateMachine)
        {
            // Add_Decimal if the variable is float or at least with decimal points
            stateMachine._sessionManager.Add_Decimal(stateMachine, "ST");
            stateMachine._sessionManager.Add_Decimal(stateMachine, "ST_2");

            string trialPackageValuesArray = ",ST:" + stateMachine._sessionManager.trialVariablesDict["ST"] + 
                                             ",ST_2:" + stateMachine._sessionManager.trialVariablesDict["ST_2"] +     
                                            ",MRN:" + stateMachine._sessionManager.trialVariablesDict["MRN"] + 
                                            ",C:" + stateMachine._sessionManager.trialVariablesDict["C"] + 
                                            ",SR:"+ stateMachine._sessionManager.trialVariablesDict["SR"] +
                                            ",DR:"+ stateMachine._sessionManager.trialVariablesDict["DR"] + 
                                            ",RF:"+ stateMachine._sessionManager.trialVariablesDict["RF"] +
                                            ",NP:"+ stateMachine._sessionManager.trialVariablesDict["NP"] + 
                                            ",GF:"+ stateMachine._sessionManager.trialVariablesDict["GF"];

            int outcome = stateMachine._sessionManager.currentRewardNum;


            if (stateMachine._sessionManager.rewardSucked)
            {
                outcome = outcome - 1;
            }

            outcome = trialStartLinearTrack.firstRewardNum * 10 + outcome;

            stateMachine._sessionManager.logEndTrial(outcome, trialPackageValuesArray);

            // write it back with real reward number
            stateMachine._sessionManager.currentRewardNum = outcome;

            stateMachine._sessionManager.trialRunning = false;
        }
    }
}
