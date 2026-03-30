using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1300/P1300_RegisterRewardZone")]

    public class P1300_RegisterRewardZone : FSMAction
    {
        public P1300_TrialStartLinearTrack trialStartLinearTrack;
        public override void Execute(BaseStateMachine stateMachine)
        {
            trialStartLinearTrack.currentRewardStateID = stateMachine.CurrentState.stateID;
        }
    }
}
