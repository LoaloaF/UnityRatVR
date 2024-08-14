using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0200/P0200_TrialEndReachedPillar")]

    public class P0200_TrialEndReachedPillar: FSMAction
    {

        public override void Execute(BaseStateMachine stateMachine)
        {

            // Ensure that the variable package is storing the float values as string
            stateMachine._sessionManager.Add_Decimal(stateMachine, "PA");
            stateMachine._sessionManager.Add_Decimal(stateMachine, "PD");


            string trialPackageValuesArray = ",PD:" + stateMachine._sessionManager.trialVariablesDict["PD"] + 
                                             ",PA:" + stateMachine._sessionManager.trialVariablesDict["PA"];
            stateMachine._sessionManager.logEndTrial(1, trialPackageValuesArray);

            
            float currentPD = float.Parse(stateMachine._sessionManager.trialVariablesDict["PD"]);
            // Auto increment of the pillar distance when the trial is successful
            if (currentPD < 45.0f)
            {
                float newPD = currentPD + 0.5f;

                if (newPD % 1 == 0)
                    stateMachine._sessionManager.trialVariablesDict["PD"] = newPD.ToString() + ".0";
                else
                    stateMachine._sessionManager.trialVariablesDict["PD"] = newPD.ToString();
                
            } else {
                // stateMachine._sessionManager.trialVariablesDict["PD"]  = "45";
            }
            Debug.Log("Distance: " + stateMachine._sessionManager.trialVariablesDict["PD"]);
            
            Color white = new Color(1, 1, 1, 1);
            stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(white);


            stateMachine._sessionManager.trialRunning = false;
        }

    }
}
