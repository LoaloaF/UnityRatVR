using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0900/P0900_TrialEndMotorLickLearning")]

    public class P0900_TrialEndMotorLickLearning: FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            
            stateMachine._sessionManager.Add_Decimal(stateMachine, "MT");
            stateMachine._sessionManager.Add_Decimal(stateMachine, "GPT");
            stateMachine._sessionManager.Add_Decimal(stateMachine, "SLT");
            stateMachine._sessionManager.Add_Decimal(stateMachine, "MTH");
            stateMachine._sessionManager.Add_Decimal(stateMachine, "STH");

            string trialPackageValuesArray =",MT:" + stateMachine._sessionManager.trialVariablesDict["MT"] + 
                                            ",ST:" + stateMachine._sessionManager.trialVariablesDict["GPT"] + 
                                            ",GPT:" + stateMachine._sessionManager.trialVariablesDict["SLT"] + 
                                            ",MTH:" + stateMachine._sessionManager.trialVariablesDict["MTH"] + 
                                            ",STH:" + stateMachine._sessionManager.trialVariablesDict["STH"] +
                                            ",MRN:" + stateMachine._sessionManager.trialVariablesDict["MRN"] +
                                            ",R:" + stateMachine._sessionManager.trialVariablesDict["R"] +
                                            ",Y:" + stateMachine._sessionManager.trialVariablesDict["Y"] +
                                            ",P:" + stateMachine._sessionManager.trialVariablesDict["P"];
                                            
            int outcome = stateMachine._sessionManager.currentRewardNum;

            if (stateMachine._sessionManager.rewardSucked)
            {
                outcome = outcome - 1;
            }
            stateMachine._sessionManager.logEndTrial(outcome, trialPackageValuesArray);

        }


    }
}
