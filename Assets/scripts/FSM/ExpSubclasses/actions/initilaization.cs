using FSM;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using RatVR.Scene;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/Initilization")]
    public class Initilization : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            UnityEngine.Debug.Log("Initilization (Action) would execture here and now.");
            // var pillarManager = stateMachine.GetComponent<PillarManager>();
            // pillarManager.CreateNewCheckpoint();


            // int childcount = stateMachine.transform.childCount;

            if (stateMachine.scene == null){
                // throw new Exception("state machine scene is null");
                UnityEngine.Debug.Log("adding state machine scene in Initilization");
                stateMachine.scene = stateMachine.GetComponent<SceneController>().scene;
            }

            // if (stateMachine.scene != null)
            {

                // var pillardata = stateMachine.scene.Pillars;
                // Vector3 pos = pillardata[childcount - 1].Position;
                // UnityEngine.Debug.Log("name from scene object data: " + pillardata[childcount - 1].UID);
            }
            // else
            {
                // throw new Exception("state machine scene is null");
                // UnityEngine.Debug.Log("state machine scene is null");
            }

        }
    }
}