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
        public override void Execute(BaseStateMachine stateMachine)
        {
            string DeathZoneAction = stateMachine._sceneController.DeathZoneAction;
            if (DeathZoneAction == "slow_down")
            {
                Vector3 gain = CalculateGain();
                 UnityEngine.Debug.Log("gain: " + gain[0] + " " + gain[1] + " " + gain[2] );
                stateMachine.player.GetComponent<PlayerMovement>().gain = gain;
            }

            Vector3 CalculateGain()
            {
                // Vector3 gain = new Vector3(0.5f, 0.5f, 0.5f);
                float[] diswall = calculatediswallRatio();
                UnityEngine.Debug.Log("diswall: "+ diswall[0] + " " + diswall[1] + " " + diswall[2] + " " + diswall[3]);
                Vector3 gain = new Vector3(1f, 1f, 1f);

               
                CharacterController controller = stateMachine.player.GetComponent<CharacterController>();
                Vector3 playerVel = controller.velocity;

                if (diswall[0] < 1f)
                {
                    if (playerVel.x >= 0)
                    {
                        gain = new Vector3(diswall[0], diswall[0], 1f);
                    }
                }

                if (diswall[1] < 1f)
                {
                    if (playerVel.x <= 0)
                    {
                        gain = new Vector3(diswall[1], diswall[1], 1f);
                    }
                }

                if (diswall[2] < 1f)
                {
                    if (playerVel.z >= 0)
                    {
                         gain = new Vector3(diswall[2], diswall[2], 1f);
                    }
                }

                if (diswall[3] < 1f)
                {
                    if (playerVel.z <= 0)
                    {
                         gain = new Vector3(diswall[3], diswall[3], 1f);
                    }
                    
                }
                return gain;
            }

            float[] calculatediswallRatio()
            {
                var scenesize = stateMachine._sceneController.scene.Size;
                Vector3 playerpos = new Vector3(stateMachine.player.transform.position.x,  stateMachine.player.transform.position.z, stateMachine.player.transform.eulerAngles.y);
                float[] diswall = new float[4];
                // check x and y in scene size
                diswall[0] = playerpos.x - scenesize.x*0.5f;
                diswall[1] = playerpos.x + scenesize.x*0.5f;
                diswall[2] = playerpos.y - scenesize.y*0.5f;
                diswall[3] = playerpos.y + scenesize.y*0.5f;

                float[] diswallRatio = {1f, 1f, 1f, 1f};
                for (int i = 0; i< diswall.Length; i++)
                {
                    if (Math.Abs(diswall[i]) < (stateMachine._sceneController.scene.DeathZone))
                    {
                        diswallRatio[i] = Math.Abs(diswall[i])/stateMachine._sceneController.scene.DeathZone;
                    }
                }

                return diswallRatio;
            }

            /*
            
            // player position, x, z, rotation 
            
            // ball input, [0]:forward, [1]: right, [2]: rotation (move relatively to the direction player is facing)
            // int[] XYZvelInput = stateMachine.player.GetComponent<PlayerMovement>().XYZvelInput;
                      
            // distance to the 4 walls
            float[] diswall = calculatediswallRatio();
            // float[] gain = CalculateGain();

            
            UnityEngine.Debug.Log("Runs when in death zone state"); 
            UnityEngine.Debug.Log("diswall: "+ diswall[0] + " " + diswall[1] + " " + diswall[2] + " " + diswall[3]);
            // UnityEngine.Debug.Log("gain: " + gain[0] + " " + gain[1] + " " + gain[2] + " " + gain[3] );

            
            // cc.move();




            
            UnityEngine.Debug.Log("x: " + cc.velocity.x + " z: " + cc.velocity.z);
            float rotation = playerpos.y;
            UnityEngine.Debug.Log("rotation: " + rotation);


            

            

            float[] CalculateGain()
            {
                float[] gain = {1f, 1f, 1f, 1f};
                for (int i = 0; i< diswall.Length; i++)
                {
                    if (Math.Abs(diswall[i]) < (stateMachine._sceneController.scene.DeathZone))
                    {
                        gain[i] = Math.Abs(diswall[i])/stateMachine._sceneController.scene.DeathZone;
                    }
                }
                return gain;
            }
            */

        }
 
    }
}
