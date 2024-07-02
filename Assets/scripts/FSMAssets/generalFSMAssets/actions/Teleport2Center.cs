using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/General/CenterTeleport")]

    public class CenterTeleport : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            if (stateMachine._sessionManager.onWallZoneEntry == "center_teleport")

            {
                float x = 0f;
                float z = 0f;
                float rot = 0f;
                stateMachine._playerMovement.TeleportRat(x, z, rot);
            }
        }

    }
}


