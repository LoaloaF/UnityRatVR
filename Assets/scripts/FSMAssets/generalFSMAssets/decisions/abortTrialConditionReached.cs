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
            if (firstDecicionCall)
            {
                t0 = Time.realtimeSinceStartup;
                currentTime = t0;
                firstDecicionCall = false;
                Debug.Log("First Call");
            }
            
            if (Time.realtimeSinceStartup-t0 > 
                stateMachine.GetComponent<SceneController>().maximumTrialLength) {
                firstDecicionCall = true;
                return true;
            }
            Debug.Log("check");
            return false;
        }

    }

}


