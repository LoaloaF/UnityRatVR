using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;



namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Decisions/RatUnderActivePillarHexagon")]
    public class RatUnderActivePillarHexagon : Decision
    {

         private float timer = 0f;
        private bool firstDecicionCall = true;
        public override bool Decide(BaseStateMachine stateMachine)
        {
   
            int childcount = stateMachine.transform.childCount;
            // Transform child = stateMachine.transform.GetChild(childcount-1);
            // List<Transform> pillars = stateMachine.GetComponentInChildren<List<Transform>>(); 
            timer += Time.deltaTime;
            for (int i = 0; i < childcount; i++)
            {
                Transform child = stateMachine.transform.GetChild(i);
                // Transform child = stateMachine.GetComponentInChildren<Transform>();
                if (child.GetComponentInChildren<PillarCollision>().PlayerDetected)
                {
                    // Debug.Log("collision detected at:" + stateMachine._sceneController.scene.Pillars[i].UID);
                    if (stateMachine._sceneController.scene.Pillars[i].IsReward == 1)
                    {
                        if (firstDecicionCall)
                        {
                            // t0 = Time.realtimeSinceStartup;
                            firstDecicionCall = false;
                        }
                        
                        if (timer > stateMachine._sessionManager.successSequenceLength) {
                            firstDecicionCall = true;
                            timer = 0f;
                            return true;
                        }
                        
                        return false;

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