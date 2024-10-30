using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0800/P0800_SuccessEndedAndTeleportBack")]
    public class P0800_SuccessEndedAndTeleportBack : Decision
    {
        public float timer = 0f;
        public P0800_TrialStartLinearTrack trialStartLinearTrack;
        public int rewardStateID;
        public override bool Decide(BaseStateMachine stateMachine)
        {            
            int cueIndicator = trialStartLinearTrack.cueIndicator;

            if (timer > stateMachine._sessionManager.successSequenceLength && rewardStateID == trialStartLinearTrack.currentRewardStateID) {
                return true;
            }
            
            timer += Time.deltaTime;
            return false;
        }

        private void OnEnable() 
        {
            timer = 0;
        }



    }

}


