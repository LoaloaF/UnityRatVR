using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0300/P0300_TrialStartHexagon")]

    public class P0300_TrialStartHexagon : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            stateMachine._sessionManager.newTrial();
            stateMachine._sessionManager.currentRewardNum= 0;
            stateMachine._sessionManager.trialRunning = true;

        }
    }
}
