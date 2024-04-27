using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/TrialStartAction")]

    public class TrialStartAction : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            stateMachine.validationSphereRenderer.enabled = false;
            stateMachine._playerMovement.EnableMovement();
        }
    }
}
