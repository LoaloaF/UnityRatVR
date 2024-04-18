using FSM;
using RatVR.Scene;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;


namespace Experiment.ExperimentFSM
{
     [CreateAssetMenu(menuName = "FSM/Decisions/InDeathZone")]
    public class InDeathZone : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            Vector2 playerPos = new Vector2(stateMachine.player.transform.position.x, stateMachine.player.transform.position.z);
            Vector2 nonDeathzone = new Vector2(stateMachine.scene.Size.x*0.5f - stateMachine.scene.DeathZone, stateMachine.scene.Size.y*0.5f - stateMachine.scene.DeathZone);
            if (Math.Abs(playerPos.x) > nonDeathzone.x || Math.Abs(playerPos.y) > nonDeathzone.y)
            {
                Debug.Log("Player is in death zone");
                return true;
            }else
            {
                stateMachine.player.GetComponent<PlayerMovement>().gain = new Vector3(1f, 1f, 1f);
                return false;
            }

        }

    }

}


