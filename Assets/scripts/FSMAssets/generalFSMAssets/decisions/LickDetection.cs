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

            while (true) {
                var portentaPackage = portentaOutputSHMInterface.PopExtractedItem();
                if (portentaPackage == null) break;

                // Debug.Log(portentaPackage["N"] + " " + portentaPackage["V"] + " " + portentaPackage["ID"]);
                if (portentaPackage["N"].ToString().Trim() == "L" && double.Parse(portentaPackage["V"].ToString()) > threshold)
                {
                    Debug.Log($"Lick above threshold detected, after {nChecks} checks");
                    nChecks = 0;
                    return true;
                }
                nChecks++;
            }
            // Debug.Log($"No lick in this frame, {nChecks} checks");
            nChecks = 0;
            return false;
        }
    }
}