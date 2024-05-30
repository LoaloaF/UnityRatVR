using FSM;
using UnityEngine;

namespace ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Decisions/LickDetection")]
    public class LickDetection : Decision
    {
        public double threshold = 200;
        private CyclicPackagesSHMInterface portentaOutputSHMInterface;
        private int nChecks = 0;
        public override bool Decide(BaseStateMachine stateMachine)
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

                // Debug.Log(portentaPackage["N"] + " " + portentaPackage["V"] + " " + portentaPackage["ID"]);
                if (portentaPackage["N"].ToString().Trim() == "L" && double.Parse(portentaPackage["V"].ToString()) > threshold)
                {
                    // Debug.Log($"Lick a  bove threshold detected, after {nChecks} checks");
                    // nChecks = 0;
                    foundLick = true;
                }
                nChecks++;
            }
        }
    }
}