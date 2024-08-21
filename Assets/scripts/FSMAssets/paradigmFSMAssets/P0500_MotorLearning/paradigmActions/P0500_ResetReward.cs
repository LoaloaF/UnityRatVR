using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0500/P0500_ResetReward")]

    public class P0500_ResetReward : FSMAction
    {
        public SuccessSequenceEnded successSequenceEnded;
        public override void Execute(BaseStateMachine stateMachine)
        {
            Color white = new Color(1, 1, 1, 1);
            stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(white);
            successSequenceEnded.timer = 0;
        }
    }
}
