using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Decisions/P0000/P0000_LickDetection")]
    public class P0000_LickDetection : Decision
    {
        public double threshold = 1;
        // private CyclicPackagesSHMInterface portentaOutputSHMInterface;
        public P0000_TrialInitAutoLickReward trialInitAutoLickReward;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            // if (portentaOutputSHMInterface == null) portentaOutputSHMInterface = new CyclicPackagesSHMInterface("portentaoutput_shmstruct.json");

            // bool foundLick = false;
            // while (true) {
            //     var portentaPackage = trialInitAutoLickReward.portentaOutputSHMInterface.PopExtractedItem();
            //     // Debug.Log(portentaPackage);

            //     // if (portentaPackage != null && portentaPackage["N"].ToString().Trim() == "L") {
            //     //     Debug.Log($"Lick a bove threshold detected, after {nChecks} checks");
            //     //     return true;
            //     // }

            //     if (portentaPackage == null) {
            //         nChecks = 0;
            //         if (foundLick) Debug.Log($"Lick a  bove threshold detected, after {nChecks} checks");
            //         return foundLick;
            //     }

            //     // Debug.Log(portentaPackage["N"] + " " + portentaPackage["V"] + " " + portentaPackage["ID"]);
            //     if (portentaPackage["N"].ToString().Trim() == "L")
            //     {
            //         Debug.Log($"Lick a  bove threshold detected, after {nChecks} checks");
            //         nChecks = 0;
            //         foundLick = true;
            //     }
            //     nChecks++;
            // }
            var portentaPackage = trialInitAutoLickReward.portentaOutputSHMInterface.PopExtractedItem();
            if (portentaPackage != null && portentaPackage["N"].ToString().Trim() == "L") 
                return true;
            else 
                return false;
                
        }


    }
}
