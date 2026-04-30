using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Decisions/P1300/P1300_RatUnderCorrectPillar")]

    public class P1300_RatUnderCorrectPillar : Decision
    {
        public P1300_TrialStartLinearTrack trialStartLinearTrack;
        public override bool Decide(BaseStateMachine stateMachine)
        {

            int childcount = stateMachine.transform.childCount;
            for (int i = 0; i < childcount; i++)
            {
                Transform child = stateMachine.transform.GetChild(i);

                if (child.name.StartsWith("Pillar" + trialStartLinearTrack.cueIndicator.ToString() + "_") && child.GetComponentInChildren<PillarCollision>().PlayerDetected)
                {
                    return true;
                }

            }

            return false;
        }

    }
}
