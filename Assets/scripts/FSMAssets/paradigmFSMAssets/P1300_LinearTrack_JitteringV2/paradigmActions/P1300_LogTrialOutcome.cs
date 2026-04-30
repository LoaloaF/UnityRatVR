using FSM;
using UnityEngine;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1300/P1300_LogTrialOutcome")]

    public class P1300_LogTrialOutcome : FSMAction
    {
        // Set per-asset in the Inspector: "success", "failure", or "" to leave that field untouched
        public string cueOutcome = "";
        public string rewardOutcome = "";

        public override void Execute(BaseStateMachine stateMachine)
        {
            if (cueOutcome != "")
                stateMachine._sessionManager.trialVariablesDict["CO"] = cueOutcome;
            if (rewardOutcome != "")
                stateMachine._sessionManager.trialVariablesDict["RO"] = rewardOutcome;
        }
    }
}
