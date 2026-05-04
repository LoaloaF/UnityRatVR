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
            stateMachine._sessionManager.Add_Decimal(stateMachine, "ST_F");
            stateMachine._sessionManager.Add_Decimal(stateMachine, "ST_L");

            string trialPackageValuesArray = ",ST_F:" + stateMachine._sessionManager.trialVariablesDict["ST_F"] +
                                             ",ST_L:" + stateMachine._sessionManager.trialVariablesDict["ST_L"] +
                                            ",C:" + stateMachine._sessionManager.trialVariablesDict["C"] +
                                            ",P_C1:"+ stateMachine._sessionManager.trialVariablesDict["P_C1"] +
                                            ",GF:"+ stateMachine._sessionManager.trialVariablesDict["GF"] +
                                            ",CO:\"" + stateMachine._sessionManager.trialVariablesDict["CO"] + "\"" +
                                            ",RO:\"" + stateMachine._sessionManager.trialVariablesDict["RO"] + "\"" +         
                                            ",P_CN:" + stateMachine._sessionManager.trialVariablesDict["P_CN"] +
                                            ",P_CM:" + stateMachine._sessionManager.trialVariablesDict["P_CM"] +
                                            ",P_CF:" + stateMachine._sessionManager.trialVariablesDict["P_CF"] +
                                            ",P_RN:" + stateMachine._sessionManager.trialVariablesDict["P_RN"] +
                                            ",P_RM:" + stateMachine._sessionManager.trialVariablesDict["P_RM"] +
                                            ",P_RF:" + stateMachine._sessionManager.trialVariablesDict["P_RF"] +
                                            ",CD:" + stateMachine._sessionManager.trialVariablesDict["CD"] +
                                            ",CR:" + stateMachine._sessionManager.trialVariablesDict["RD"];
                                            
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
