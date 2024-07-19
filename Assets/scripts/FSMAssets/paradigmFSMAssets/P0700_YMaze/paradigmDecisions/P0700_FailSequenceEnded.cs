using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0700/P0700_FailSequenceEnded")]
    public class P0700_FailSequenceEnded : Decision
    {
        private float timer = 0f;
        private int currentTrialID = -1;
        [SerializeField] private float failTime;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            if (currentTrialID != stateMachine._sessionManager._currentTrialID)
            {
                currentTrialID = stateMachine._sessionManager._currentTrialID;
                timer = 0f;
            }
            
            if (timer > failTime) {
                timer = 0f;
                return true;
            }
            
            timer += Time.deltaTime;
            return false;
        }

        private void OnEnable() 
        {
            timer = 0;
            currentTrialID = -1;
        }



    }

}


