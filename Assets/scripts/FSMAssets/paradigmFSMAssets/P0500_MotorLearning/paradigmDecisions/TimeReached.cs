using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0500/TimeReached")]
    public class TimeReached : Decision
    {
        private float timer = 0f;
        private int currentTrialID = -1;
        [SerializeField] private string timeVariableName;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            int movementTime = int.Parse(stateMachine._sessionManager.trialVariablesDict[timeVariableName]);

            if (currentTrialID != stateMachine._sessionManager._currentTrialID)
            {
                currentTrialID = stateMachine._sessionManager._currentTrialID;
                timer = 0f;
            }
            
            if (timer > movementTime) {
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


