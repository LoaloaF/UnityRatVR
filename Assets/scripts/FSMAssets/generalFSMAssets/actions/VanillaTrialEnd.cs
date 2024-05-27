using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/VanillaTrialEnd")]

    public class VanillaTrialEnd : FSMAction
    {
        public MeshRenderer validationSphereRenderer; // MeshRenderer object that you can assign in the UI

        public override void Execute(BaseStateMachine stateMachine)
        {
            
            // which Pillar was reached
            stateMachine._sessionManager.logEndTrial();
            
            Color white = new Color(1, 1, 1, 1);
            stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(white);
        }

    }
}
