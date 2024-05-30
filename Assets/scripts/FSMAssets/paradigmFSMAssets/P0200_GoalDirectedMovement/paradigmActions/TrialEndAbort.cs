using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/TrialEndAbort")]

    public class TrialEndAbort: FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            
            // which Pillar was reached
            // string[] trialPackageVariablesArray = stateMachine._sessionManager.trialPackageVariables.Split(',');
            string trialPackageValuesArray = ",TD:" + stateMachine._sessionManager.trialVariablesDict["TD"] + ",TA:" + stateMachine._sessionManager.trialVariablesDict["TA"];
            stateMachine._sessionManager.logEndTrial(0, trialPackageValuesArray);
            
            Color white = new Color(1, 1, 1, 1);
            stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(white);
        }

    }
}
