using FSM;
using UnityEngine;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1300/P1300_LogZoneOutcomes")]

    public class P1300_LogZoneOutcomes: FSMAction
    {
        // Set per-asset in the Inspector: "success", "failure", or "" to leave that field untouched
        public string cueZoneOutcome = "";
        public string rewardZoneOutcome = "";
        public P1300_TrialStartLinearTrack trialStartLinearTrack;
        public override void Execute(BaseStateMachine stateMachine)
        {
            if (cueZoneOutcome != "")
            {
                stateMachine._sessionManager.trialLogDict["CO"] = cueZoneOutcome;
            }
            if (rewardZoneOutcome != "")
            {
                stateMachine._sessionManager.trialLogDict["RO"] = rewardZoneOutcome;
                if (rewardZoneOutcome == "stopped")
                {
                    if (trialStartLinearTrack.cueIndicator == 1)
                        stateMachine._sessionManager.trialLogDict["CC"] = "1";
                    else
                        stateMachine._sessionManager.trialLogDict["CC"] = "0";
                }
                else if (rewardZoneOutcome == "skipped")
                {
                    if (trialStartLinearTrack.cueIndicator == 1)
                        stateMachine._sessionManager.trialLogDict["CC"] = "0";
                    else
                        stateMachine._sessionManager.trialLogDict["CC"] = "1";
                }
            }

        }
    }
}
