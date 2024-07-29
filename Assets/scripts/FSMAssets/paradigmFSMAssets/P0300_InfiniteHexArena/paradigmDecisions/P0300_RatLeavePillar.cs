using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0300/P0300_RatLeavePillar")]
    public class P0300_RatLeavePillar : Decision
    {

        public override bool Decide(BaseStateMachine stateMachine)
        {
            int childcount = stateMachine.transform.childCount;

            for (int i = 0; i < childcount; i++)
            {
                Transform child = stateMachine.transform.GetChild(i);
                // Transform child = stateMachine.GetComponentInChildren<Transform>();
                if (child.GetComponentInChildren<PillarCollision>().PlayerDetected)
                {
                    // Debug.Log("collision detected at:" + stateMachine._sceneController.scene.Pillars[i].UID);
                    if (stateMachine._sceneController.scene.Pillars[i].IsReward == 1)
                    {
                        // Debug.Log("player hits a rewarding pillar");
                        return false;
                    }
                }

            }

            return true;
        }

    }

}


