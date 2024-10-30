using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0500/P0500_ClearSHMInput")]

    public class P0500_ClearSHMInput : FSMAction
    {
        public P0500_TrialInitMotorLearning trialInitMotorLearning;

        public override void Execute(BaseStateMachine stateMachine)
        {
            while (trialInitMotorLearning.portentaOutputSHMInterface.Popitem() != null)
            {
                Debug.Log("Clearing SHM input");
            }
        }
    }
}
