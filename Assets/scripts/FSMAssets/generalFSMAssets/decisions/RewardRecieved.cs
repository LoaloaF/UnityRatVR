using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Decisions/RewardRecieved")]

    public class RewardRecieved : Decision
    {
        private float timer;
        public override bool Decide(BaseStateMachine stateMachine)
        {

            // if reward is recieved 
            timer += Time.realtimeSinceStartup;
            
            if (timer > 1)
            {
                Debug.Log("Time over 1s");
                timer = 0f;
                return true;
            }

            return false;

        }

    }

}
