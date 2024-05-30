using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/AbortTrialConditionReached")]
    public class AbortTrialConditionReached : Decision
    {
        private float timer;
        private float currentTime;
        // private bool firstDecicionCall = true;
        public override bool Decide(BaseStateMachine stateMachine)
        {            
            if (Time.realtimeSinceStartup-stateMachine._sessionManager.trialStartTimestamp > 
                stateMachine._sessionManager.maximumTrialLength) {
                return true;
            }

            timer += Time.deltaTime;
            return false;
        }
    }
}


