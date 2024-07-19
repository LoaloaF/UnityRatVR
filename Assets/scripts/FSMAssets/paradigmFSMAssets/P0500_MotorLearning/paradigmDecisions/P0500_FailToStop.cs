using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0500/FailToStop")]
    public class P0500_FailToStop : Decision
    {
        public P0500_MovementInQueue movementInQueue;
        private float stopTime;
        private float timer = 0f;
        private int currentTrialID = -1;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            stopTime = float.Parse(stateMachine._sessionManager.trialVariablesDict["ST"]);
            if (currentTrialID != stateMachine._sessionManager._currentTrialID)
            {
                currentTrialID = stateMachine._sessionManager._currentTrialID;
                timer = 0f;
            }
            
            if (timer > stopTime) {
                float moveThreshold = float.Parse(stateMachine._sessionManager.trialVariablesDict["STH"]);
                float movementSum = Mathf.Abs(movementInQueue.rawMovementTemp) + Mathf.Abs(movementInQueue.yawMovementTemp) + Mathf.Abs(movementInQueue.pitchMovementTemp);

                if (movementSum > moveThreshold)
                {
                    timer = 0f;
                    return true;
                }
                else
                {
                    return false;
                }

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


