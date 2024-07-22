using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Decisions/P0800/P0800_RatUnderCorrectPillar")]

    public class P0800_RatUnderCorrectPillar : Decision
    {
        public P0800_TrialStartLinearTrack trialStartLinearTrack;

        public override bool Decide(BaseStateMachine stateMachine)
        {
            int childcount = stateMachine.transform.childCount;
            for (int i = 0; i < childcount; i++)
            {
                Transform child = stateMachine.transform.GetChild(i);

                if (child.name.StartsWith("Pillar" + trialStartLinearTrack.cueIndicator.ToString()) && child.GetComponentInChildren<PillarCollision>().PlayerDetected)
                {
                    Debug.Log("Rat under correct pillar " + child.name);
                    return true;
                }

            }
            return false;
        }

    }
}
