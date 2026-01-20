using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Decisions/P1200/P1200_RatUnderCorrectPillar")]

    public class P1200_RatUnderCorrectPillar : Decision
    {
        public P0800_TrialStartLinearTrack trialStartLinearTrack;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            int doubleReward = int.Parse(stateMachine._sessionManager.trialVariablesDict["DR"]);

            int childcount = stateMachine.transform.childCount;
            for (int i = 0; i < childcount; i++)
            {
                Transform child = stateMachine.transform.GetChild(i);

                if (doubleReward == 1)
                {
                    if ((child.name.StartsWith("Pillar3_") || child.name.StartsWith("Pillar4_")) && child.GetComponentInChildren<PillarCollision>().PlayerDetected)
                    {
                        return true;
                    }
                }
                else if (child.name.StartsWith("Pillar" + trialStartLinearTrack.cueIndicator.ToString() + "_") && child.GetComponentInChildren<PillarCollision>().PlayerDetected)
                {
                    return true;
                }

            }

            return false;
        }

    }
}
