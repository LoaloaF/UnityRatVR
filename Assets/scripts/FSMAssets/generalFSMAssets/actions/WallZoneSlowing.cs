using FSM;
using System;
using System.Diagnostics;
using UnityEngine;
using RatVR.Scene;


namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Actions/SlowDownAction")]
    public class SlowDownAction : FSMAction
    {
        private Vector3 caculatedGain;
        private float[] SlowRatio = new float[4] { 1f, 1f, 1f, 1f };

        public override void Execute(BaseStateMachine stateMachine)
        {
            string WallZoneAction = stateMachine._sessionManager.onWallZoneEntry;
            if (WallZoneAction == "slow_down")
            {
                CalculateGain(stateMachine);
                stateMachine.Player.GetComponent<PlayerMovement>().gain = caculatedGain;
            }
        }

        private void CalculateGain(BaseStateMachine stateMachine)
        {
            caculatedGain = new Vector3(1f, 1f, 1f);

            // Calculate the slow ratio for each direction
            SlowRatio = CalculatediswallRatio(stateMachine);
            
            CharacterController controller = stateMachine.Player.GetComponent<CharacterController>();
            Vector3 playerVel = controller.velocity;

            // If the player is approaching the right wall:
            // if the player is moving towards the right wall, slow down the player in the x direction; if not, make the gain 1
            // But keep the z direction gain as what it is (to make sure proper behavior in the corner)
            if (SlowRatio[0] < 1f)
            {
                if (playerVel.x >= 0)
                    caculatedGain = new Vector3(SlowRatio[0], 1f, caculatedGain.z);
                else
                    caculatedGain = new Vector3(1f, 1f, caculatedGain.z);
            }

            // same logic for the left wall
            if (SlowRatio[1] < 1f)
            {
                if (playerVel.x <= 0)
                    caculatedGain = new Vector3(SlowRatio[1], 1f, caculatedGain.z);
                else
                    caculatedGain = new Vector3(1f, 1f, caculatedGain.z);
            }

            // same logic for the top wall
            if (SlowRatio[2] < 1f)
            {
                if (playerVel.z >= 0)
                    caculatedGain = new Vector3(caculatedGain.x, 1f, SlowRatio[2]);
                else
                    caculatedGain = new Vector3(caculatedGain.x, 1f, 1f);
            }

            // same logic for the bottom wall
            if (SlowRatio[3] < 1f)
            {
                if (playerVel.z <= 0)
                    caculatedGain = new Vector3(caculatedGain.x, 1f, SlowRatio[3]);
                else
                    caculatedGain = new Vector3(caculatedGain.x, 1f, 1f);
            }
        }

        private float[] CalculatediswallRatio(BaseStateMachine stateMachine)
        {
            var scenesize = stateMachine._sceneController.scene.Size;
            Vector3 playerpos = new Vector3(stateMachine.Player.transform.position.x,  stateMachine.Player.transform.position.z, stateMachine.Player.transform.eulerAngles.y);

            float[] diswallRatio = new float[4] { 1f, 1f, 1f, 1f };
            float[] diswall = new float[4] { 0f, 0f, 0f, 0f };

            // Calculate the distance to the walls
            diswall[0] = playerpos.x - scenesize.x * 0.5f * stateMachine._sceneController.scene.BaseLength; // to right wall
            diswall[1] = playerpos.x + scenesize.x * 0.5f * stateMachine._sceneController.scene.BaseLength; // to left wall
            diswall[2] = playerpos.y - scenesize.y * 0.5f * stateMachine._sceneController.scene.BaseLength; // to top wall
            diswall[3] = playerpos.y + scenesize.y * 0.5f * stateMachine._sceneController.scene.BaseLength; // to bottom wall


            for (int i = 0; i< diswall.Length; i++)
            {
                if (Math.Abs(diswall[i]) < (stateMachine._sceneController.scene.WallZone * stateMachine._sceneController.scene.BaseLength))
                {
                    // The speed ratio will be 1 at the entry of the wall zone
                    // and decrease linearly to 0 at the stop distance (wallZoneStopDistance)
                    diswallRatio[i] = (1 - (stateMachine._sceneController.scene.WallZone * stateMachine._sceneController.scene.BaseLength - Math.Abs(diswall[i]))/stateMachine._playerMovement.wallZoneStopDistance * stateMachine._sceneController.scene.BaseLength)/4;
                }
                else
                {
                    diswallRatio[i] = 1f;
                }
            }

            return diswallRatio;
        }
    }
}
