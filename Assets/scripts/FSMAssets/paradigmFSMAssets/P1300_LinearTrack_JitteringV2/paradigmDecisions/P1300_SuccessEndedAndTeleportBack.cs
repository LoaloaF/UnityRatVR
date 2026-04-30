using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P1300/P1300_SuccessEndedAndTeleportBack")]
    public class P1300_SuccessEndedAndTeleportBack : Decision
    {
        public float timer = 0f;
        public P1300_TrialStartLinearTrack trialStartLinearTrack;
        public override bool Decide(BaseStateMachine stateMachine)
        {            
            if (timer > stateMachine._sessionManager.successSequenceLength) {
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


