using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0200/P0200_TrialEndAbort")]

    public class P0200_TrialEndAbort: FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            
            // Ensure that the variable package is storing the float values as string
            float currentPD = float.Parse(stateMachine._sessionManager.trialVariablesDict["PD"]);
            float currentPA = float.Parse(stateMachine._sessionManager.trialVariablesDict["PA"]);

            if (currentPD % 1 == 0)
                stateMachine._sessionManager.trialVariablesDict["PD"] = currentPD.ToString() + ".0";
            else
                stateMachine._sessionManager.trialVariablesDict["PD"] = currentPD.ToString();

            if (currentPA % 1 == 0)
                stateMachine._sessionManager.trialVariablesDict["PA"] = currentPA.ToString() + ".0";
            else
                stateMachine._sessionManager.trialVariablesDict["PA"] = currentPA.ToString();


            string trialPackageValuesArray = ",PD:" + stateMachine._sessionManager.trialVariablesDict["PD"] + ",PA:" + stateMachine._sessionManager.trialVariablesDict["PA"];
            stateMachine._sessionManager.logEndTrial(0, trialPackageValuesArray);
            
            Color white = new Color(1, 1, 1, 1);
            stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(white);
            stateMachine._sessionManager.trialRunning = false;
        }

    }
}
