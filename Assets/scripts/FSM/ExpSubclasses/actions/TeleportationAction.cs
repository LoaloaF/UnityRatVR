using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/TeleportationAction")]

    public class TeleportationAction : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
             string DeathZoneAction = stateMachine.GetComponent<SceneController>().DeathZoneAction;
            if (DeathZoneAction == "teleportation")
            {
                teleportation();
            }
            
            void teleportation()
            {
                Debug.Log("runs when in Teleportation action");
                float x = 0f;
                float z = 0f;
                float rot = 0f;
                // Debug.Log("Teleporting Rat" + "x: " + x + "z: " + z + "rotation: " + rot);
                stateMachine.player.GetComponent<PlayerMovement>().TeleportRat(x, z, rot);
            }


        }

    }
}


