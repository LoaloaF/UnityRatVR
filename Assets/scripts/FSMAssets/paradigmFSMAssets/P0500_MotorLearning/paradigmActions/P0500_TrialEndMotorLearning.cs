using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0500/P0500_TrialEndMotorLearning")]

    public class P0500_TrialEndMotorLearning: FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            
            stateMachine._sessionManager.Add_Decimal(stateMachine, "MT");
            stateMachine._sessionManager.Add_Decimal(stateMachine, "ST");
            stateMachine._sessionManager.Add_Decimal(stateMachine, "GPT");
            stateMachine._sessionManager.Add_Decimal(stateMachine, "MTH");
            stateMachine._sessionManager.Add_Decimal(stateMachine, "STH");
            stateMachine._sessionManager.Add_Decimal(stateMachine, "SSTH");

            string trialPackageValuesArray =",MT:" + stateMachine._sessionManager.trialVariablesDict["MT"] + 
                                            ",ST:" + stateMachine._sessionManager.trialVariablesDict["ST"] + 
                                            ",GPT:" + stateMachine._sessionManager.trialVariablesDict["GPT"] + 
                                            ",MTH:" + stateMachine._sessionManager.trialVariablesDict["MTH"] + 
                                            ",STH:" + stateMachine._sessionManager.trialVariablesDict["STH"] +
                                            ",SSTH:" + stateMachine._sessionManager.trialVariablesDict["SSTH"] +
                                            ",MRN:" + stateMachine._sessionManager.trialVariablesDict["MRN"] +
                                            ",R:" + stateMachine._sessionManager.trialVariablesDict["R"] +
                                            ",Y:" + stateMachine._sessionManager.trialVariablesDict["Y"] +
                                            ",P:" + stateMachine._sessionManager.trialVariablesDict["P"] +
                                            ",LR:" + stateMachine._sessionManager.trialVariablesDict["LR"];

            stateMachine._sessionManager.logEndTrial(stateMachine._sessionManager.currentRewardNum, trialPackageValuesArray);

        }


    }
}
