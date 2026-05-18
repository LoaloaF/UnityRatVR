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
            stateMachine._sessionManager.Add_Decimal(stateMachine, "ST_O");

            string trialPackageValuesArray = ",ST_F:" + stateMachine._sessionManager.trialVariablesDict["ST_F"] +
                                             ",ST_O:" + stateMachine._sessionManager.trialVariablesDict["ST_O"] +
                                            ",C:" + stateMachine._sessionManager.trialLogDict["C"] +
                                            ",P_C1:"+ stateMachine._sessionManager.trialVariablesDict["P_C1"] +
                                            ",GF:"+ stateMachine._sessionManager.trialVariablesDict["GF"] +
                                            ",CO:\"" + stateMachine._sessionManager.trialLogDict["CO"] + "\"" +
                                            ",CO_FID:" + stateMachine._sessionManager.trialLogDict["CO_FID"] +
                                            ",CO_PCT:" + stateMachine._sessionManager.trialLogDict["CO_PCT"] +
                                            ",RO:\"" + stateMachine._sessionManager.trialLogDict["RO"] + "\"" +
                                            ",RO_FID:" + stateMachine._sessionManager.trialLogDict["RO_FID"] +
                                            ",RO_PCT:" + stateMachine._sessionManager.trialLogDict["RO_PCT"] +
                                            ",TO:" + stateMachine._sessionManager.trialLogDict["TO"] +
                                            ",P_CN:" + stateMachine._sessionManager.trialVariablesDict["P_CN"] +
                                            ",P_CM:" + stateMachine._sessionManager.trialVariablesDict["P_CM"] +
                                            ",P_CF:" + stateMachine._sessionManager.trialVariablesDict["P_CF"] +
                                            ",P_RN:" + stateMachine._sessionManager.trialVariablesDict["P_RN"] +
                                            ",P_RM:" + stateMachine._sessionManager.trialVariablesDict["P_RM"] +
                                            ",P_RF:" + stateMachine._sessionManager.trialVariablesDict["P_RF"] +
                                            ",CZ_P:" + stateMachine._sessionManager.trialLogDict["CZ_P"] +
                                            ",RZ_P:" + stateMachine._sessionManager.trialLogDict["RZ_P"] +
                                            ",CZV_FID:" + stateMachine._sessionManager.trialLogDict["CZV_FID"] +
                                            ",CZV_PCT:" + stateMachine._sessionManager.trialLogDict["CZV_PCT"] +
                                            ",RZV_FID:" + stateMachine._sessionManager.trialLogDict["RZV_FID"] +
                                            ",RZV_PCT:" + stateMachine._sessionManager.trialLogDict["RZV_PCT"];
                                            
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
