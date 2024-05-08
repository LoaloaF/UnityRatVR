using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/AbortSequenceEnded")]
    public class AbortSequenceEnded : Decision
    {
        private float timer = 0f;
        private bool firstDecicionCall = true;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            if (firstDecicionCall)
            {
                // t0 = Time.realtimeSinceStartup;
                firstDecicionCall = false;
            }
            
            if (timer > stateMachine._sessionManager.abortInterTrialIntervalLength) {
                firstDecicionCall = true;
                timer = 0f;
                return true;
            }
            
            timer += Time.deltaTime;
            return false;
        }

    }

}


