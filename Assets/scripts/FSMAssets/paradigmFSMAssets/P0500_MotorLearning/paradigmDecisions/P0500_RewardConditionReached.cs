using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0500/P0500_RewardConditionReached")]
    public class P0500_RewardConditionReached : Decision
    {
        public P0500_MovementInQueue movementInQueue;
        public override bool Decide(BaseStateMachine stateMachine)
        {

        }


        private bool CheckStop(BaseStateMachine stateMachine)
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

        private bool LickReward(BaseStateMachine stateMachine)
        {
            if (portentaOutputSHMInterface == null) portentaOutputSHMInterface = new CyclicPackagesSHMInterface("portentaoutput_shmstruct.json");

            bool foundLick = false;
            while (true) {
                var portentaPackage = portentaOutputSHMInterface.PopExtractedItem();

                if (portentaPackage == null) {
                    nChecks = 0;
                    if (foundLick) Debug.Log($"Lick a  bove threshold detected, after {nChecks} checks");
                    return foundLick;
                }

                if (portentaPackage["N"].ToString().Trim() == "L")
                {
                    Debug.Log($"Lick a  bove threshold detected, after {nChecks} checks");
                    nChecks = 0;
                    foundLick = true;
                }
                nChecks++;
            }
        }

    }

}


