using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1300/P1300_ClearSHMInput")]

    public class P1300_ClearSHMInput : FSMAction
    {
        public P1300_TrialInitLinearTrack trialInitLinearTrack;

        public override void Execute(BaseStateMachine stateMachine)
        {
            while (trialInitLinearTrack.portentaOutputSHMInterface.Popitem() != null);
        }
    }
}
