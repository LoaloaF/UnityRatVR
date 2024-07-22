using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;



namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Decisions/P0800/P0800_ReachEnd")]
    public class P0800_ReachEnd : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            if (stateMachine._playerMovement.transform.position.z > 140f)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}