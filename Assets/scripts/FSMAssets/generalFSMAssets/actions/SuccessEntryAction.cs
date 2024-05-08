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
            int rewardDelay = stateMachine._sessionManager.rewardPostSoundDelay;
            int rewardLength = stateMachine._sessionManager.rewardAmount;



            Debug.Log("sending success, reward delay: " + rewardDelay + " reward length: " + rewardLength + " seconds.");
           
            stateMachine.GetComponent<PortentaInputInterface>().sendSuccess(rewardDelay, rewardLength);

            Color yellow = new Color(1, 1, 0, 1);
            stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(yellow);
            // foreach (Transform child in stateMachine._sceneController.Lighting.transform)
            // {
            //     Light light = child.GetComponent<Light>();
            //     if (light != null)
            //     {
            //         light.color = new Color(0, 0, 0, 1); // replace r, g, b, a with the color values you want to use
            //     }
            // }
        }

    }
}
