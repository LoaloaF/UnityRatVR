using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;



namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Decisions/P1300/P1300_IsCue1Shown")]
    public class P1300_IsCue1Shown : Decision
    {
        public P1300_TrialStartLinearTrack trialStartLinearTrack;

        // Assumes RF is always 1 in the P1300 excel:
        //   cueIndicator == 4 → cue1 (go), cueIndicator == 3 → cue2 (no-go)
        public override bool Decide(BaseStateMachine stateMachine)
        {
            return trialStartLinearTrack.cueIndicator == 4;
        }
    }
}