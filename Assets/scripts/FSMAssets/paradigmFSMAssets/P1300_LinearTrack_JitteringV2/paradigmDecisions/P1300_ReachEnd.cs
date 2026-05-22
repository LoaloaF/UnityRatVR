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

            float Arenalength = stateMachine._sceneController.scene.BaseLength * stateMachine._sceneController.scene.Size.y;
            if (stateMachine._playerMovement.transform.position.z >= Arenalength/2f - 10f)
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