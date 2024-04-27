using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/InterTrialIntervalEnded")]
    public class InterTrialIntervalEnded : Decision
    {
        private float t0;
        private float currentTime;
        private bool firstDecicionCall = true;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            if (firstDecicionCall)
            {
                t0 = Time.realtimeSinceStartup;
                currentTime = t0;
                firstDecicionCall = false;
            }
            
            Debug.Log(Time.realtimeSinceStartup);
            Debug.Log(t0);
            if (Time.realtimeSinceStartup-t0 > 
                stateMachine._sessionManager.interTrialIntervalTrialLength) {
                firstDecicionCall = true;
                return true;
            }
            return false;
        }

    }

}


