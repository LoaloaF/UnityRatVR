using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/CenterTeleport")]

    public class CenterTeleport : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            string DeathZoneAction = stateMachine._sceneController.DeathZoneAction;
            if (DeathZoneAction == "center_teleport")
            {
                float x = 0f;
                float z = 0f;
                float rot = 0f;
                stateMachine.player.GetComponent<PlayerMovement>().TeleportRat(x, z, rot);
            }
            else
            {
                Debug.Log($"DeathZoneAction `{DeathZoneAction}` != `center_teleport`");
            }
        }

    }
}


