using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0500/P0500_SuccessUnderPillar")]

    public class P0500_SuccessUnderPillar : FSMAction
    {
        public P0500_TrialStartMotorLearning trialStartMotorLearning;
        public override void Execute(BaseStateMachine stateMachine)
        {
            int rewardDelay = stateMachine._sessionManager.rewardPostSoundDelay;
            int rewardLength = stateMachine._sessionManager.rewardAmount;

            trialStartMotorLearning.currentRewardNumber++;
            Debug.Log("sending success, reward delay: " + rewardDelay + " reward length: " + rewardLength + " seconds.");
            stateMachine.GetComponent<PortentaInputInterface>().sendSuccess(rewardDelay, rewardLength);
            Color yellow = new Color(1, 1, 0, 1);
            stateMachine._sceneController.Lighting.GetComponent<globalLightController>().switchSceneColor(yellow);
            
        }

    }
}
