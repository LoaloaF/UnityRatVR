using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/TrialStartHexagon")]

    public class TrialStartHexagon : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            stateMachine._sessionManager.newTrial();
            stateMachine._sessionManager.trialVariablesDict["RN"] = "0";
            stateMachine._sessionManager.trialRunning = true;

        }
    }
}
