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
        private bool lickReward = false;
        private CyclicPackagesSHMInterface portentaOutputSHMInterface;
        private int nChecks = 0;

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


