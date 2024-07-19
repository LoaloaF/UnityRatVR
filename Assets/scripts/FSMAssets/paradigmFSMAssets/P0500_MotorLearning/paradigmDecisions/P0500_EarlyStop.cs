using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0500/EarlyStop")]
    public class P0500_EarlyStop : Decision
    {
        public P0500_MovementInQueue movementInQueue;
        public P0500_TrialStartMotorLearning trialStartMotorLearning;
        public TimeReached moveTimeReached;
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
                trialStartMotorLearning.rawMovementQueue.Clear();
                trialStartMotorLearning.yawMovementQueue.Clear();
                trialStartMotorLearning.pitchMovementQueue.Clear();
                trialStartMotorLearning.rawMovementQueue.Enqueue(0);
                trialStartMotorLearning.yawMovementQueue.Enqueue(0);
                trialStartMotorLearning.pitchMovementQueue.Enqueue(0);
                moveTimeReached.timer = 0;
                return true;
            }
        }

    }

}


