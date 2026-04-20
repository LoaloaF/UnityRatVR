using FSM;
using UnityEngine;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1300/P1300_LogTrialOutcome")]

    public class P1300_LogTrialOutcome : FSMAction
    {
        // Set per-asset in the Inspector: "success" or "failure"
        public string outcome = "";
        public P1300_TrialStartLinearTrack trialStartLinearTrack;

        public override void Execute(BaseStateMachine stateMachine)
        {
            int trialID = stateMachine._sessionManager._currentTrialID;
            string cue = trialStartLinearTrack.cueIndicator == 4 ? "cue1" : "cue2";
            string cueOutcome = cue + "_" + outcome;

            stateMachine._sessionManager.trialVariablesDict["RO"] = outcome;
            stateMachine._sessionManager.trialVariablesDict["CO"] = cueOutcome;

            if (outcome == "success")
                stateMachine._sessionManager.successfulTrials += 1;
            else if (outcome == "failure")
                stateMachine._sessionManager.failedTrials += 1;
            else
                Debug.LogWarning("P1300_LogTrialOutcome: outcome not set correctly — expected 'success' or 'failure', got '" + outcome + "'");

            Debug.Log("Trial " + trialID + ": " + cueOutcome);
        }
    }
}
