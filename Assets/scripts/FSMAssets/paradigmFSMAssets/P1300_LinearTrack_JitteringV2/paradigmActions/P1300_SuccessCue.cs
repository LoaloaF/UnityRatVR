using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1300/P1300_SuccessCue")]

    public class P1300_SuccessCue : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
                Debug.Log("SuccessCue");
                UnityEngine.Color yellow = new UnityEngine.Color(1, 1, 0, 1);
                stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(yellow);
                AudioSource reward_beep = stateMachine._playerMovement.GetComponent<AudioSource>();
                reward_beep.Play();
                UnityEngine.Color white = new UnityEngine.Color(1, 1, 1, 1);
                stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(white);
        }

    }
}
