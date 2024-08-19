using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0500/P0500_FailStopInStayStop")]
    public class P0500_FailStopInStayStop : Decision
    {
        public P0500_MovementInQueue movementInQueue;
        public override bool Decide(BaseStateMachine stateMachine)
        {

            float moveThreshold = float.Parse(stateMachine._sessionManager.trialVariablesDict["SSTH"]);
            // float movementSum = Mathf.Abs(movementInQueue.rawMovementTemp) + Mathf.Abs(movementInQueue.yawMovementTemp) + Mathf.Abs(movementInQueue.pitchMovementTemp);
            float movementSum = Mathf.Abs(movementInQueue.rawMovementTemp);

            if (movementSum > moveThreshold)
            {
                Debug.Log("FailStopInStayStop");
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}


