using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0500/FailToStop")]
    public class FailToStop : Decision
    {
        public MovementInQueue movementInQueue;
        [SerializeField] private float stopTime;
        private float timer = 0f;
        private int currentTrialID = -1;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            if (currentTrialID != stateMachine._sessionManager._currentTrialID)
            {
                currentTrialID = stateMachine._sessionManager._currentTrialID;
                timer = 0f;
            }
            
            if (timer > stopTime) {
                float moveThreshold = float.Parse(stateMachine._sessionManager.trialVariablesDict["STH"]);
                float movementSum = movementInQueue.rawMovementTemp + movementInQueue.yawMovementTemp + movementInQueue.pitchMovementTemp;

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


