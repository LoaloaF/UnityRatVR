using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0800/P0800_TrialEndLinearTrack")]

    public class P0800_TrialEndLinearTrack : FSMAction
    {
        public P0800_TrialStartLinearTrack trialStartLinearTrack;
        public override void Execute(BaseStateMachine stateMachine)
        {
            float variableST = float.Parse(stateMachine._sessionManager.trialVariablesDict["ST"]);
            if (variableST % 1 == 0)
                stateMachine._sessionManager.trialVariablesDict["ST"] = variableST.ToString() + ".0";
            else
                stateMachine._sessionManager.trialVariablesDict["ST"] = variableST.ToString();

            string trialPackageValuesArray = ",ST:" + stateMachine._sessionManager.trialVariablesDict["ST"] + 
                                            ",MRN:" + stateMachine._sessionManager.trialVariablesDict["MRN"] + 
                                            ",C:" + stateMachine._sessionManager.trialVariablesDict["C"] + 
                                            ",LR:"+ stateMachine._sessionManager.trialVariablesDict["LR"];

            int outcome = int.Parse(stateMachine._sessionManager.trialVariablesDict["RN"]);

            if (trialStartLinearTrack.trialSuccess)
                stateMachine._sessionManager.logEndTrial(outcome, trialPackageValuesArray);
            else
                stateMachine._sessionManager.logEndTrial(outcome, trialPackageValuesArray);

            stateMachine._sessionManager.trialRunning = false;
        }
    }
}
