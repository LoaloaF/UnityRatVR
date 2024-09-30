using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0800/P0800_SuckReward")]

    public class P0800_SuckReward : FSMAction
    {
        private bool isSucking = false;
        private float suckTimer = 0f;

        public override void Execute(BaseStateMachine stateMachine)
        {
            if (!stateMachine.pumpOpened)
            {
                return;
            }
            if (isSucking)
            {
                suckTimer += Time.deltaTime;
                if (suckTimer >= 0.5f)
                {
                    suckTimer = 0;
                    isSucking = false;
                }
            }
            else
            {
                stateMachine.serialPort.Write("run\n");
                isSucking = true;
            }
        }

    }
}
