using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/General/SessionEnded")]
    public class SessionEnded : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            if (stateMachine._sessionManager.abortTrialFlag) {
                if (stateMachine._sessionManager.trialRunning) {
                    Debug.Log("SessionEnded while trial was running");
                    stateMachine._sessionManager.logEndTrial();
                    stateMachine._sessionManager.trialRunning = false;
                } else {
                    Debug.Log("SessionEnded while in ITI");
                }
                return true;
            }
            return false;
        }
    }
}


