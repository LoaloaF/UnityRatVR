using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/P0800/P0800_SuccessSequenceEnded")]
    public class P0800_SuccessSequenceEnded : Decision
    {
        public float timer = 0f;
        public override bool Decide(BaseStateMachine stateMachine)
        {            
            if (timer > stateMachine._sessionManager.successSequenceLength) {
                return true;
            }
            
            timer += Time.deltaTime;
            return false;
        }

        private void OnEnable() 
        {
            timer = 0;
        }



    }

}


