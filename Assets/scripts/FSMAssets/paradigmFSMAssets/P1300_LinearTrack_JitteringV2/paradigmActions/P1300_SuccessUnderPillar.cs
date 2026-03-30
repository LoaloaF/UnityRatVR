using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1300/P1300_SuccessUnderPillar")]

    public class P1300_SuccessUnderPillar : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            int rewardDelay = stateMachine._sessionManager.rewardPostSoundDelay;
            int rewardLength = stateMachine._sessionManager.rewardAmount;

            int maxRewardNum = int.Parse(stateMachine._sessionManager.trialVariablesDict["MRN"]);

            if (stateMachine._sessionManager.currentRewardNum < maxRewardNum)
            {
                stateMachine._sessionManager.currentRewardNum++;
                Debug.Log("sending success, reward delay: " + rewardDelay + " reward length: " + rewardLength + " seconds.");
                stateMachine.GetComponent<PortentaInputInterface>().sendSuccess(rewardDelay, rewardLength);
                // Color yellow = new Color(1, 1, 0, 1);
                // stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(yellow);
                stateMachine._sessionManager.rewardPresent = true;
            }
            else
            {
                Debug.Log("Maximum number of rewards reached. Not sending reward.");
                Color white = new Color(1, 1, 1, 1);
                stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(white);
            }

        }

    }
}
