using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/TrialEndAction")]

    public class TrialEndAction : FSMAction
    {
        public MeshRenderer validationSphereRenderer; // MeshRenderer object that you can assign in the UI

        public override void Execute(BaseStateMachine stateMachine)
        {
            // Debug.Log("TrialEndAction");
            
            // which Pillar was reached
            stateMachine._sessionManager.logEndTrial("P:1");

        }

    }
}
