using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;



namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Decisions/P1300/P1300_RatLeaveRewardPillar")]
    public class P1300_RatLeaveRewardPillar : Decision
    {

        [SerializeField] private string pillarIdentifier;
        public P1300_TrialStartLinearTrack trialStartLinearTrack;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            int childcount = stateMachine.transform.childCount;
            
            for (int i = 0; i < childcount; i++)
            {
                Transform child = stateMachine.transform.GetChild(i);

                if (!child.name.StartsWith("Pillar" + pillarIdentifier + "_"))
                    continue;

                if (!child.GetComponentInChildren<PillarCollision>().PlayerDetected)
                    return true;
                else
                    return false;
            }

            return false;
        }
    }
}