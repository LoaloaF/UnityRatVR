using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0500/EarlyStop")]
    public class EarlyStop : Decision
    {
        public MovementInQueue movementInQueue;
        public TrialStartMotorLearning trialStartMotorLearning;
        public TimeReached stopTimeReached;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            float moveThreshold = float.Parse(stateMachine._sessionManager.trialVariablesDict["STH"]);
            float movementSum = movementInQueue.rawMovementTemp + movementInQueue.yawMovementTemp + movementInQueue.pitchMovementTemp;

            if (movementSum > moveThreshold)
            {
                trialStartMotorLearning.rawMovementQueue.Clear();
                trialStartMotorLearning.yawMovementQueue.Clear();
                trialStartMotorLearning.pitchMovementQueue.Clear();
                stopTimeReached.timer = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}


