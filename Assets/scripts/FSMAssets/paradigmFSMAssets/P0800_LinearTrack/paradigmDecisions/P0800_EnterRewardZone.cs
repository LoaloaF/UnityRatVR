using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;



namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Decisions/P0800/P0800_EnterRewardZone")]
    public class P0800_EnterRewardZone : Decision
    {
        public P0800_TrialStartLinearTrack trialStartLinearTrack;
        public override bool Decide(BaseStateMachine stateMachine)
        {
            float enterRewardPosZ = trialStartLinearTrack.enterRewardPosZ;
            float endRewardPosZ = trialStartLinearTrack.endRewardPosZ;

            if (stateMachine._playerMovement.transform.position.z >= enterRewardPosZ)
            {
                Debug.Log("Entered Reward Zone");
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}