using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/General/VanillaTrialStart")]

    public class VanillaTrialStart : FSMAction
    {     
        public override void Execute(BaseStateMachine stateMachine)
        {
            stateMachine._sessionManager.newTrial();
            Debug.Log("New trial started");
        }
    }
}
