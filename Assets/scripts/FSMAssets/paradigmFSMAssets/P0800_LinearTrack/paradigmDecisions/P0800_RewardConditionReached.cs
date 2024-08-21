using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0800/P0800_RewardConditionReached")]
    public class P0800_RewardConditionReached : Decision
    {
        public float timer = 0f;
        public float threshold = 23;
        public P0800_TrialInitLinearTrack trialInitLinearTrack;

        public override bool Decide(BaseStateMachine stateMachine)
        {
            int lickRewardCheck = int.Parse(stateMachine._sessionManager.trialVariablesDict["LR"]);

            if (lickRewardCheck == 1)
            {
                return LickReward(stateMachine);
            }
            else
                return StayTimeReached(stateMachine);

        }

        private bool LickReward(BaseStateMachine stateMachine)
        {
            

            // bool foundLick = false;
            // while (true) {
            //     var portentaPackage = trialInitLinearTrack.portentaOutputSHMInterface.PopExtractedItem();

            //     if (portentaPackage == null) {
            //         nChecks = 0;
            //         if (foundLick) Debug.Log($"Lick a  bove threshold detected, after {nChecks} checks");
            //         return foundLick;
            //     }

            //     Debug.Log(portentaPackage["V"]);
            //     if (portentaPackage["N"].ToString().Trim() == "L")
            //     {
            //         // if (int.Parse(portentaPackage["V"].ToString()) > threshold) {
            //         Debug.Log($"Lick a  bove threshold detected, after {nChecks} checks");
            //         nChecks = 0;
            //         foundLick = true;
            //         }
            //     }
            //     nChecks++;
            // }

            var portentaPackage = trialInitLinearTrack.portentaOutputSHMInterface.PopExtractedItem();
            if (portentaPackage != null && portentaPackage["N"].ToString().Trim() == "L") 
                return true;
            else 
                return false;
        }


        private bool StayTimeReached(BaseStateMachine stateMachine)
        {
            float movementTime = float.Parse(stateMachine._sessionManager.trialVariablesDict["ST"]);
            
            if (timer > movementTime) {
                return true;
            }
            
            timer += Time.deltaTime;
            return false;
        }

        private void OnEnable() 
        {
            timer = 0;
        }



    }

}


