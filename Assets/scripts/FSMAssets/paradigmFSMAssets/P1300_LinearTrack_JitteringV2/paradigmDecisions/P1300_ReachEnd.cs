using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;



namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Decisions/P1300/P1300_ReachEnd")]
    public class P1300_ReachEnd : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            if (stateMachine._playerMovement.transform.position.z > 260f)
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