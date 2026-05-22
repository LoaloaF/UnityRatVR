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
                                            ",CID:" + stateMachine._sessionManager.trialLogDict["CID"] +
                                            ",P_CID1:" + stateMachine._sessionManager.trialVariablesDict["P_CID1"] +
                                            ",G_F:" + stateMachine._sessionManager.trialVariablesDict["G_F"] +
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
                                            ",CZ_POS:" + stateMachine._sessionManager.trialLogDict["CZ_POS"] +
                                            ",RZ_POS:" + stateMachine._sessionManager.trialLogDict["RZ_POS"] +
                                            ",CZV_FID:" + stateMachine._sessionManager.trialLogDict["CZV_FID"] +
                                            ",CZV_PCT:" + stateMachine._sessionManager.trialLogDict["CZV_PCT"] +
                                            ",RZV_FID:" + stateMachine._sessionManager.trialLogDict["RZV_FID"] +
                                            ",RZV_PCT:" + stateMachine._sessionManager.trialLogDict["RZV_PCT"] +
                                            ",CC:" + stateMachine._sessionManager.trialLogDict["CC"];
;
                                            
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
