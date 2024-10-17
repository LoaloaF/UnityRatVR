using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0800/P0800_ResetReward")]

    public class P0800_ResetReward : FSMAction
    {
        public P0800_RewardConditionReached rewardConditionReached;
        public P0800_SuccessEndedAndTeleportBack successReward1;
        public P0800_SuccessEndedAndTeleportBack successReward2;
        public override void Execute(BaseStateMachine stateMachine)
        {
            Color white = new Color(1, 1, 1, 1);
            stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(white);
            // rewardConditionReached.timer = 0;
            successReward1.timer = 0;
            successReward2.timer = 0;
        }
    }
}
