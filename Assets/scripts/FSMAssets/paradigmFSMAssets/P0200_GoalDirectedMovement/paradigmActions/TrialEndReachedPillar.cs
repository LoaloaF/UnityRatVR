using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/TrialEndReachedPillar")]

    public class TrialEndReachedPillar: FSMAction
    {

        public override void Execute(BaseStateMachine stateMachine)
        {
            // which Pillar was reached
            // string[] trialPackageVariablesArray = stateMachine._sessionManager.trialPackageVariables.Split(',');
            // [TD,TA]
            // ... fill with values that were used in this trial
            string trialPackageValuesArray = ",PD:" + stateMachine._sessionManager.trialVariablesDict["PD"] + ",PA:" + stateMachine._sessionManager.trialVariablesDict["PA"];
            stateMachine._sessionManager.logEndTrial(1, trialPackageValuesArray);
            
            Color white = new Color(1, 1, 1, 1);
            stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(white);
        }

    }
}
