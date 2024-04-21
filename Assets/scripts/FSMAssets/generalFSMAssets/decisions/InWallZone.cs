using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;

namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/InWallZone")]
    public class InWallZone : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            Vector2 playerPos = new Vector2(stateMachine.player.transform.position.x, 
                                            stateMachine.player.transform.position.z);
            // why 0.5f scaling?
            Vector2 nonDeathzone = new Vector2(stateMachine._sceneController.scene.Size.x*0.5f - stateMachine._sceneController.scene.DeathZone,
                                               stateMachine._sceneController.scene.Size.y*0.5f - stateMachine._sceneController.scene.DeathZone);
            return (Math.Abs(playerPos.x) > nonDeathzone.x || Math.Abs(playerPos.y) > nonDeathzone.y);
        }

    }

}


