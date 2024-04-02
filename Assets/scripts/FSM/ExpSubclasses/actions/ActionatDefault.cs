using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/ActionAtDefault")]

    public class ActionatDefault : FSMAction
    {
        // public GameObject player;
        public override void Execute(BaseStateMachine stateMachine)
        {
            Debug.Log("Runs when in default state");
            // Debug.Log(stateMachine.transform.GetChild(0).name);

            // we can have the access to the pillars by getting the child game object
            int childcount = stateMachine.transform.childCount;

            //child.transform.localPosition = Vector3.zero;
                        // Debug.Log(stateMachine.mapath);
            // we can get the pillar data from the excel sheet, by index

            /*
            if (stateMachine.scene != null)
            {
                
                var pillardata = stateMachine.scene.Pillars;
                Vector3 pos = pillardata[childcount - 1].Position;
                
                Debug.Log("name from scene object data: " + pillardata[childcount-1].UID);
            }
            else
            {
                Debug.Log("state machine scene is null");
            }
            */
        

            // var pillarManager = stateMachine.GetComponent<PillarManager>();

            // pillarManager.CreateNewCheckpoint();

        }
    }
}


