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
        public P0800_StayTimeReached stayTimeReached;
        public P0800_SuccessSequenceEnded successSequenceEnded;
        public override void Execute(BaseStateMachine stateMachine)
        {
            Color white = new Color(1, 1, 1, 1);
            stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(white);
            stayTimeReached.timer = 0;
            successSequenceEnded.timer = 0;
        }
    }
}
