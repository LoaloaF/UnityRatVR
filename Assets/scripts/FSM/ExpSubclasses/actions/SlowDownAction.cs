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
            string DeathZoneAction = stateMachine.GetComponent<SceneController>().DeathZoneAction;
            if (DeathZoneAction == "slow down")
            {
                Vector3 gain = CalculateGain();
                stateMachine.player.GetComponent<PlayerMovement>().gain = gain;
            }

            Vector3 CalculateGain()
            {
                Vector3 gain = new Vector3(0.5f, 0.5f, 0.5f);
                return gain;
            }

            /*
            var scenesize = stateMachine.scene.Size;
            // player position, x, z, rotation 
            Vector3 playerpos = new Vector3(stateMachine.player.transform.position.x,  stateMachine.player.transform.position.z, stateMachine.player.transform.eulerAngles.y);
            // ball input, [0]:forward, [1]: right, [2]: rotation (move relatively to the direction player is facing)
            // int[] XYZvelInput = stateMachine.player.GetComponent<PlayerMovement>().XYZvelInput;
                      
            // distance to the 4 walls
            float[] diswall = calculatediswallRatio();
            // float[] gain = CalculateGain();

            
            UnityEngine.Debug.Log("Runs when in death zone state"); 
            UnityEngine.Debug.Log("diswall: "+ diswall[0] + " " + diswall[1] + " " + diswall[2] + " " + diswall[3]);
            // UnityEngine.Debug.Log("gain: " + gain[0] + " " + gain[1] + " " + gain[2] + " " + gain[3] );

            CharacterController cc = stateMachine.player.GetComponent<CharacterController>();
            // cc.move();




            
            UnityEngine.Debug.Log("x: " + cc.velocity.x + " z: " + cc.velocity.z);
            float rotation = playerpos.y;
            UnityEngine.Debug.Log("rotation: " + rotation);


            float[] calculatediswallRatio()
            {
                float[] diswall = new float[4];
                // check x and y in scene size
                diswall[0] = playerpos.x - scenesize.x*0.5f;
                diswall[1] = playerpos.x + scenesize.x*0.5f;
                diswall[2] = playerpos.y - scenesize.y*0.5f;
                diswall[3] = playerpos.y + scenesize.y*0.5f;

                float[] diswallRatio = {1f, 1f, 1f, 1f};
                for (int i = 0; i< diswall.Length; i++)
                {
                    if (Math.Abs(diswall[i]) < (stateMachine.scene.DeathZone))
                    {
                        diswallRatio[i] = Math.Abs(diswall[i])/stateMachine.scene.DeathZone;
                    }
                }

                return diswallRatio;
            }

            

            float[] CalculateGain()
            {
                float[] gain = {1f, 1f, 1f, 1f};
                for (int i = 0; i< diswall.Length; i++)
                {
                    if (Math.Abs(diswall[i]) < (stateMachine.scene.DeathZone))
                    {
                        gain[i] = Math.Abs(diswall[i])/stateMachine.scene.DeathZone;
                    }
                }
                return gain;
            }
            */

        }
 
    }
}
