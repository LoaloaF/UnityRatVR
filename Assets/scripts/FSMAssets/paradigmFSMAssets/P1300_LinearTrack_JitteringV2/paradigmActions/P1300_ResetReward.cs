using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1300/P1300_ResetReward")]

    public class P1300_ResetReward : FSMAction
    {
        public P1300_RewardConditionReached rewardConditionReached;
        public P1300_SuccessEndedAndTeleportBack successReward1;
        public P1300_SuccessEndedAndTeleportBack successReward2;
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
