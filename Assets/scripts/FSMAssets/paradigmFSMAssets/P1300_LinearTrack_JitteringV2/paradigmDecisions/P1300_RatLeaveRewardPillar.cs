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
        [SerializeField] private string zoneDefinition;
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
                {   
                    if (zoneDefinition == "cueZone"){
                        if (stateMachine._sessionManager.trialLogDict["CO_FID"] == "")
                        {
                            stateMachine._sessionManager.trialLogDict["CO_FID"] = Time.frameCount.ToString();
                            stateMachine._sessionManager.trialLogDict["CO_PCT"] = stateMachine._sessionManager.getUnixTimestampMicroseconds().ToString();
                        }
                    }
                    else if (zoneDefinition == "rewardZone"){
                        if (stateMachine._sessionManager.trialLogDict["RO_FID"] == "")
                        {
                            stateMachine._sessionManager.trialLogDict["RO_FID"] = Time.frameCount.ToString();
                            stateMachine._sessionManager.trialLogDict["RO_PCT"] = stateMachine._sessionManager.getUnixTimestampMicroseconds().ToString();
                        }
                    }
                    return true;

                }
                else
                    return false;
            }

            return false;
        }
    }
}