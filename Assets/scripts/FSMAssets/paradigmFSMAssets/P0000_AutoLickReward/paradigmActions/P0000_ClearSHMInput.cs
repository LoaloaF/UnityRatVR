using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0000/P0000_ClearSHMInput")]

    public class P0000_ClearSHMInput : FSMAction
    {
        public P0000_TrialInitAutoLickReward trialInitAutoLickReward;

        public override void Execute(BaseStateMachine stateMachine)
        {
            while (trialInitAutoLickReward.portentaOutputSHMInterface.Popitem() != null);
        }
    }
}
