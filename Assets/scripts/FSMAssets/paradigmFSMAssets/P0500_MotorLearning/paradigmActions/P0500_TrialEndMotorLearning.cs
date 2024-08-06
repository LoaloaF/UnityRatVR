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
        [SerializeField] int successIndicator;
        public override void Execute(BaseStateMachine stateMachine)
        {

            string trialPackageValuesArray =",MT:" + stateMachine._sessionManager.trialVariablesDict["MT"] + 
                                            ",ST:" + stateMachine._sessionManager.trialVariablesDict["ST"] + 
                                            ",MTH:" + stateMachine._sessionManager.trialVariablesDict["MTH"] + 
                                            ",STH:" + stateMachine._sessionManager.trialVariablesDict["STH"] +
                                            ",R:" + stateMachine._sessionManager.trialVariablesDict["R"] +
                                            ",Y:" + stateMachine._sessionManager.trialVariablesDict["Y"] +
                                            ",P:" + stateMachine._sessionManager.trialVariablesDict["P"] +
                                            ",LR:" + stateMachine._sessionManager.trialVariablesDict["LR"];
            stateMachine._sessionManager.logEndTrial(successIndicator, trialPackageValuesArray);

        }

    }
}
