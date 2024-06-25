using System.Collections;
using FSM;
using System;
using System.Diagnostics;
using UnityEngine;
using RatVR.Scene;


namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Actions/PillarBlinking")]
    public class PillarBlinking : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            int childcount = stateMachine.transform.childCount;
            float frequency = 10f;

            for (int i = 0; i < childcount; i++){

                Renderer pillarRenderer = stateMachine.transform.GetChild(i).GetComponentInChildren<Renderer>();

                float blinkInterval = 1/frequency;
                bool isvisible = (Mathf.Floor(Time.time / blinkInterval) % 2 == 0);
                pillarRenderer.enabled = isvisible;


            }
            
        }
 
    }
}


