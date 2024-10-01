using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0300/P0300_SuccessUnderPillar")]

    public class P0300_SuccessUnderPillar : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            int rewardDelay = stateMachine._sessionManager.rewardPostSoundDelay;
            int rewardLength = stateMachine._sessionManager.rewardAmount;

            int maxRewardNum = int.Parse(stateMachine._sessionManager.trialVariablesDict["MRN"]);
            int currentRewardNum = int.Parse(stateMachine._sessionManager.trialVariablesDict["RN"]);


            if (currentRewardNum < maxRewardNum)
            {
                currentRewardNum++;
                stateMachine._sessionManager.trialVariablesDict["RN"] = currentRewardNum.ToString();
                Debug.Log("sending success, reward delay: " + rewardDelay + " reward length: " + rewardLength + " seconds.");
                stateMachine.GetComponent<PortentaInputInterface>().sendSuccess(rewardDelay, rewardLength);
                Color yellow = new Color(1, 1, 0, 1);
                stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(yellow);
                stateMachine.rewardPresent = true;
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
