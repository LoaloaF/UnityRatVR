using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/General/SuckReward")]

    public class SuckReward : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            if (!stateMachine.pumpOpened)
            {
                return;
            }
            if (stateMachine.rewardPresent)
            {
                stateMachine.serialPort.Write("run\n");
                stateMachine.rewardPresent = false;
            }
        }

    }
}
