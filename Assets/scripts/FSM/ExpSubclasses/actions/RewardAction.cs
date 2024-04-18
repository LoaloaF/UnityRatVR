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
        private float timer;
        private bool successsent = false;
        public override void Execute(BaseStateMachine stateMachine)
        {
            Debug.Log("Runs when in reward action");
            Debug.Log("timer: " + timer);
            
            if (! successsent)
            {
                Debug.Log("play sound, give reward");
                int rewardDelay = stateMachine.scene.RewardDelay;
                int rewardLength = stateMachine.scene.RewardLength;
            
                // stateMachine.GetComponent<PortentaInputInterface>().sendSuccess(rewardDelay, rewardLength);
                Debug.Log("sending success, reward delay: " + rewardDelay + " reward length: " + rewardLength + " seconds.");
                successsent = true;                               
            }
            
            if (timer > 5f)
            {
                Debug.Log("resetting timer");
                timer = 0f;
                successsent = false;
            }
            timer += Time.deltaTime;



        }

    }
}
