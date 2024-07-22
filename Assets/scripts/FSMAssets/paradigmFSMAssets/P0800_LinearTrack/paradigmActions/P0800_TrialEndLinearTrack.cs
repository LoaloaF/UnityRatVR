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
            string trialPackageValuesArray = ",MRN:" + stateMachine._sessionManager.trialVariablesDict["MRN"] + ",RN:" + stateMachine._sessionManager.trialVariablesDict["RN"];

            if (trialStartLinearTrack.trialSuccess)
                stateMachine._sessionManager.logEndTrial(1, trialPackageValuesArray);
            else
                stateMachine._sessionManager.logEndTrial(0, trialPackageValuesArray);

            stateMachine._sessionManager.trialRunning = false;
        }
    }
}
