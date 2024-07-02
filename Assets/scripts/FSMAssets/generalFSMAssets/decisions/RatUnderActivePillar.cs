using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;



namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Decisions/General/RatUnderActivePillar")]
    public class RatUnderActivePillar : Decision
    {

        public override bool Decide(BaseStateMachine stateMachine)
        {

 

            int childcount = stateMachine.transform.childCount;
            // Transform child = stateMachine.transform.GetChild(childcount-1);
            // List<Transform> pillars = stateMachine.GetComponentInChildren<List<Transform>>(); 

            for (int i = 0; i < childcount; i++)
            {
                Transform child = stateMachine.transform.GetChild(i);
                // Transform child = stateMachine.GetComponentInChildren<Transform>();
                if (child.GetComponentInChildren<PillarCollision>().PlayerDetected)
                {
                    // Debug.Log("collision detected at:" + stateMachine._sceneController.scene.Pillars[i].UID);
                    if (stateMachine._sceneController.scene.Pillars[i].IsReward == 1)
                    {
                        Debug.Log("player hits a rewarding pillar");

                        return true;

                    }
                    else
                    {
                        Debug.Log("player hits a non-rewarding pillar");
                        return false;
                        
                    }
                }

            }

            return false;
            // var pillarManager = stateMachine.GetComponent<PillarManager>();
            // return pillarManager.CheckPlayerPillar();
        }
    }
}