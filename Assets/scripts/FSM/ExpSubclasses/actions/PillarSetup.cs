using FSM;
using System;
using System.Diagnostics;
using UnityEngine;
using RatVR.Scene;


namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Actions/PillarSetup")]
    public class PillarSetup : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            UnityEngine.Debug.Log("Runs when setting up new pillar"); 
            


        }
    }
}
