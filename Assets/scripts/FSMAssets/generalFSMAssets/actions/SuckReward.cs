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
 
            if (stateMachine._sessionManager.rewardPresent)
            {
                try
                {
                    stateMachine.serialPort.Write("run\n");
                    stateMachine.GetComponent<PortentaInputInterface>().sendRewardSuction(20);
                    Debug.Log("Sucking reward");
                    stateMachine._sessionManager.rewardPresent = false;
                    stateMachine._sessionManager.rewardSucked = true;
                }
                catch (System.Exception e)
                {
                    Debug.Log("Error: " + e);
                }

            }
        }

    }
}
