using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0500/TrialEndMotorLearning")]

    public class TrialEndMotorLearning: FSMAction
    {
        [SerializeField] int successIndicator;
        public override void Execute(BaseStateMachine stateMachine)
        {

            string trialPackageValuesArray = ",MT:" + stateMachine._sessionManager.trialVariablesDict["MT"] + 
                                             ",ST:" + stateMachine._sessionManager.trialVariablesDict["ST"] + 
                                             ",MTH:" + stateMachine._sessionManager.trialVariablesDict["MTH"] + 
                                             ",STH:" + stateMachine._sessionManager.trialVariablesDict["STH"];
            stateMachine._sessionManager.logEndTrial(successIndicator, trialPackageValuesArray);

        }

    }
}
