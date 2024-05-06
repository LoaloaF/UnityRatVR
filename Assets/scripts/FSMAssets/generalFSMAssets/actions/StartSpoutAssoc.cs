using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/StartSpoutAssoc")]

    public class StartSpoutAssoc : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {         
            // stateMachine.validationSphereRenderer.enabled = true;
            string packValues = "";
            //stateMachine._sessionManager.logNewTrial(packValues);
            Debug.Log("session manager, log new trial");
            stateMachine._sessionManager.trialRunning = true;

        }

    }
}
