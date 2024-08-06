using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0800/P0800_ClearSHMInput")]

    public class P0800_ClearSHMInput : FSMAction
    {
        public P0800_TrialInitLinearTrack trialInitLinearTrack;

        public override void Execute(BaseStateMachine stateMachine)
        {
            while (trialInitLinearTrack.portentaOutputSHMInterface.Popitem() != null);
        }
    }
}
