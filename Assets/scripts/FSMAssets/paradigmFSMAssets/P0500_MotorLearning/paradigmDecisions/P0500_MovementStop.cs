using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0500/MovementStop")]
    public class P0500_MovementStop : Decision
    {
        public P0500_MovementInQueue movementInQueue;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            float moveThreshold = float.Parse(stateMachine._sessionManager.trialVariablesDict["STH"]);
            float movementSum = Mathf.Abs(movementInQueue.rawMovementTemp) + Mathf.Abs(movementInQueue.yawMovementTemp) + Mathf.Abs(movementInQueue.pitchMovementTemp);

            if (movementSum > moveThreshold)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }

}


