using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0800/P0800_RegisterRewardZone")]

    public class P0800_RegisterRewardZone : FSMAction
    {
        public P0800_TrialStartLinearTrack trialStartLinearTrack;
        public override void Execute(BaseStateMachine stateMachine)
        {
            trialStartLinearTrack.currentRewardStateID = stateMachine.LastState.stateID;
        }
    }
}
