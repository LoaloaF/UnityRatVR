using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0700/P0700_TrialEndSuccessYMaze")]

    public class P0700_TrialEndSuccessYMaze: FSMAction
    {

        public override void Execute(BaseStateMachine stateMachine)
        {

            // string trialPackageValuesArray = ",PD:" + stateMachine._sessionManager.trialVariablesDict["PD"] + ",PA:" + stateMachine._sessionManager.trialVariablesDict["PA"];
            string trialPackageValuesArray = "";
            stateMachine._sessionManager.logEndTrial(1, trialPackageValuesArray);

            
            Color white = new Color(1, 1, 1, 1);
            stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(white);

            stateMachine._sessionManager.trialRunning = false;
        }

    }
}
