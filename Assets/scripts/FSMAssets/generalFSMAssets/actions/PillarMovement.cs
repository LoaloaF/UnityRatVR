using System.Collections;
using FSM;
using System;
using System.Diagnostics;
using UnityEngine;
using RatVR.Scene;


namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Actions/PillarMovement")]
    public class PillarMovement : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            int childcount = stateMachine.transform.childCount;
            float height = 5f;
            float frequency = 1f;

            for (int i = 0; i < childcount; i++){

                Transform child = stateMachine.transform.GetChild(i);

                Vector3 pillarPosition = child.position;

                // use the height of pillar when initializing the pillar
                float pillarHeight = stateMachine._sceneController.scene.Pillars[i].Height + stateMachine._sceneController.scene.Pillars[i].Position.z;
                
                float newY = Mathf.Sin(2* Mathf.PI*Time.time * frequency) * height + pillarHeight;

                child.position = new Vector3(pillarPosition.x, newY, pillarPosition.z);
                // UnityEngine.Debug.Log("Pillar moved" + newY);

            }
            
        }
 
    }
}



