using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/abortTrialConditionReached")]
    public class abortTrialConditionReached : Decision
    {
        private float t0;
        private float currentTime;
        private bool firstDecicionCall = true;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            // Debug.Log("Time.realtimeSinceStartup:"+Time.realtimeSinceStartup+ " t0:"+t0+" firstDecicionCall: "+firstDecicionCall);
            if (firstDecicionCall)
            {
                // Debug.Log("FIRSTabortTrialConditionReached");
                t0 = Time.realtimeSinceStartup;
                currentTime = t0;
                firstDecicionCall = false;
            }
            
            Debug.Log(Time.realtimeSinceStartup-t0);
            Debug.Log(stateMachine._sessionManager.maximumTrialLength);
            
            if (Time.realtimeSinceStartup-t0 > 
                stateMachine._sessionManager.maximumTrialLength) {
                firstDecicionCall = true;
                return true;
            }
            return false;
        }

    }

}


