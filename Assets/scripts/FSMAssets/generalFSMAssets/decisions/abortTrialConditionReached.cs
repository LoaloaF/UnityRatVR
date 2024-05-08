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
        private float timer;
        private float currentTime;
        private bool firstDecicionCall = true;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            // Debug.Log("Time.realtimeSinceStartup:"+Time.realtimeSinceStartup+ " t0:"+t0+" firstDecicionCall: "+firstDecicionCall);
            if (firstDecicionCall)
            {
                // Debug.Log("FIRSTabortTrialConditionReached");
                timer = 0f;
                firstDecicionCall = false;
            }
            
            if (timer > stateMachine._sessionManager.maximumTrialLength) {
                firstDecicionCall = true;
                timer = 0f;
                return true;
            }

            timer += Time.deltaTime;
            return false;
        }

    }

}


