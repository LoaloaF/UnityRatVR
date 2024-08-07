using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0000/P0000_TrialInitAutoLickReward")]

    public class P0000_TrialInitAutoLickReward : FSMAction
    {
        public CyclicPackagesSHMInterface portentaOutputSHMInterface;

        public override void Execute(BaseStateMachine stateMachine)
        {
            if (portentaOutputSHMInterface == null) portentaOutputSHMInterface = new CyclicPackagesSHMInterface("portentaoutput_shmstruct.json");
        }
    }
}
