using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0600/P00600_Start")]

    public class P00600_Start : FSMAction
    {
        private float pillarAngle;
     
        public override void Execute(BaseStateMachine stateMachine)
        {
            stateMachine._sessionManager.newTrial();
            stateMachine._playerMovement.EnableMovement();
            stateMachine.validationSphereRenderer.enabled = false;
        }
    }
}
