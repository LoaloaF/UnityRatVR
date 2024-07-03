using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0300/TrialEndHexagon")]

    public class TrialEndHexagon: FSMAction
    {

        public override void Execute(BaseStateMachine stateMachine)
        {

            string trialPackageValuesArray = ",MRN:" + stateMachine._sessionManager.trialVariablesDict["MRN"] + ",RN:" + stateMachine._sessionManager.trialVariablesDict["RN"];
            stateMachine._sessionManager.logEndTrial(1, trialPackageValuesArray);


    
            
            Color white = new Color(1, 1, 1, 1);
            stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(white);
            stateMachine._sessionManager.trialRunning = false;
        }

    }
}
