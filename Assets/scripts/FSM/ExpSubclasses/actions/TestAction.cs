using FSM;
using System;
using System.Diagnostics;
using UnityEngine;
using RatVR.Scene;


namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Actions/TestAction")]
    public class TestAction : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            UnityEngine.Debug.Log("Runs when in TestAction"); 


        }
    }
}
