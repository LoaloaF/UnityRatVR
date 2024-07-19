using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0700/P0700_TrialStartYMaze")]

    public class P0700_TrialStartYMaze : FSMAction
    {
        public int cueOnRight = -1;
        public override void Execute(BaseStateMachine stateMachine)
        {

            Vector3 newStartPosition = new Vector3(0, 0, -95f);
            stateMachine._playerMovement.TeleportRat(newStartPosition.x, newStartPosition.z, newStartPosition.y);

            stateMachine._sceneController.floor.SetActive(true);
            stateMachine._sceneController.wallZone.SetActive(true);
            stateMachine.validationSphereRenderer.enabled = false;
            stateMachine._sceneController.wallTop.SetActive(true);
            stateMachine._sceneController.wallBottom.SetActive(true);
            stateMachine._sceneController.wallLeft.SetActive(true);
            stateMachine._sceneController.wallRight.SetActive(true);
            stateMachine._playerMovement.EnableMovement();


            foreach (Transform child in stateMachine.transform)
            {
                child.gameObject.SetActive(true);
            }


            GameObject[] pillars = GameObject.FindGameObjectsWithTag("Pillar");
            float randomValue = Random.Range(0f, 1f);

            if (randomValue > 0.5f)
            {
                cueOnRight = 1;
            }
            else
            {
                cueOnRight = 0;
            }
            Debug.Log("cueOnRight: " + cueOnRight);

            int pillarCount = 0;
            foreach (GameObject pillar in pillars)
            {
                if (pillar.name.StartsWith("Pillar2") && pillar.transform.position.x > 0)
                {
                    stateMachine._sceneController.scene.Pillars[pillarCount].IsReward = cueOnRight;
                    pillar.GetComponentInChildren<PillarCollision>().PlayerDetected = false;
                }
                else if (pillar.name.StartsWith("Pillar2") && pillar.transform.position.x < 0)
                {
                    stateMachine._sceneController.scene.Pillars[pillarCount].IsReward = 1 - cueOnRight;
                    pillar.GetComponentInChildren<PillarCollision>().PlayerDetected = false;

                }
                else if (pillar.transform.position.x > 0 && pillar.transform.position.z < 0 && cueOnRight == 1)
                {
                    foreach(var meshRenderer in pillar.gameObject.GetComponentsInChildren<MeshRenderer>(true))
                    {  
                        if(meshRenderer.name == "Cylinder")
                        {
                            meshRenderer.material.color = new Color(1,1,1,1);
                        }
                    }
                }
                else if (pillar.transform.position.x < 0 && pillar.transform.position.z < 0 && cueOnRight == 0)
                {
                    foreach(var meshRenderer in pillar.gameObject.GetComponentsInChildren<MeshRenderer>(true))
                    {  
                        if(meshRenderer.name == "Cylinder")
                        {
                            meshRenderer.material.color = new Color(1,1,1,1);
                        }
                    }
                }
                else
                {
                    foreach(var meshRenderer in pillar.gameObject.GetComponentsInChildren<MeshRenderer>(true))
                    {  
                        if(meshRenderer.name == "Cylinder")
                        {
                            meshRenderer.material.color = new Color(0,0,0,1);
                        }
                    }
                }
                
                pillarCount++;
 
            }
      
            stateMachine._sessionManager.newTrial();
            stateMachine._sessionManager.trialRunning = true;


        }
    }
}
