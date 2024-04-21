using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/RewardAction")]

    public class RewardAction : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            int rewardDelay = stateMachine._sceneController.scene.RewardDelay;
            int rewardLength = stateMachine._sceneController.scene.RewardLength;
           
            stateMachine.GetComponent<PortentaInputInterface>().sendSuccess(rewardDelay, rewardLength);
        }

    }
}
