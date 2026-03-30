using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P1300/P1300_TrialInitLinearTrack")]

    public class P1300_TrialInitLinearTrack : FSMAction
    {
        public GameObject trackWall;
        public P1300_FadeInCue fadeInCue1;
        public P1300_FadeInCue fadeInCue2;
        public CyclicPackagesSHMInterface portentaOutputSHMInterface;
        public Dictionary<string, MeshRenderer> pillarCylinderMeshes = new Dictionary<string, MeshRenderer>();
        public float ballSidewaysNormToCentimeter;
        public float ballRotatationNormToCentimeter;

        public override void Execute(BaseStateMachine stateMachine)
        {
            stateMachine._sceneController.floor.SetActive(true);
            stateMachine._sceneController.wallZone.SetActive(true);
            stateMachine.validationSphereRenderer.enabled = false;
            stateMachine._sceneController.ceiling.SetActive(true);
            stateMachine._sceneController.wallTop.SetActive(true);
            stateMachine._sceneController.wallBottom.SetActive(true);
            stateMachine._sceneController.wallLeft.SetActive(true);
            stateMachine._sceneController.wallRight.SetActive(true);
            stateMachine._playerMovement.EnableMovement();

            if (portentaOutputSHMInterface == null) portentaOutputSHMInterface = new CyclicPackagesSHMInterface("portentaoutput_shmstruct.json");

            ballSidewaysNormToCentimeter = stateMachine._playerMovement.ballSidewaysNormToCentimeter;
            ballRotatationNormToCentimeter = stateMachine._playerMovement.ballRotatationNormToCentimeter;
            stateMachine._playerMovement.ballSidewaysNormToCentimeter = 0f;
            stateMachine._playerMovement.ballRotatationNormToCentimeter = 0f;


            stateMachine._playerMovement.zOnlyMovePositive = true;

            GameObject[] pillars = GameObject.FindGameObjectsWithTag("Pillar");
            Debug.Log("Pillar count: " + pillars.Length);

            for (int pillarIdx = 1; pillarIdx < 17; pillarIdx++)
            {
                foreach (GameObject pillar in pillars)
                {   
                    if (pillar.name.StartsWith("Pillar" + pillarIdx.ToString() + "_"))
                    {
                        MeshRenderer[] meshRenderer = pillar.GetComponentsInChildren<MeshRenderer>();
                        foreach (MeshRenderer mesh in meshRenderer)
                        {
                            if (mesh.gameObject.name == "Cylinder")
                            {
                                pillarCylinderMeshes.Add(pillarIdx.ToString() , mesh);
                            }

                        }
                        break;
                    }
                    
                }
            }
 

            MeshRenderer pillar3Mesh = pillarCylinderMeshes["3"];
            MeshRenderer pillar4Mesh = pillarCylinderMeshes["4"];
            pillar3Mesh.material.mainTextureScale = new Vector2(3.37f, 3.37f);
            pillar4Mesh.material.mainTextureScale = new Vector2(3.37f, 3.37f);


            foreach (GameObject pillar in pillars)
            {
                if (pillar.name.StartsWith("Pillar0_"))
                {
                    pillar.SetActive(false);
                }

                else if (pillar.name.StartsWith("Pillar1_")|| pillar.name.StartsWith("Pillar2_"))
                {
                    MeshRenderer[] meshRenderer = pillar.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer mesh in meshRenderer)
                    {
                        if (pillar.name.StartsWith("Pillar1_"))
                        {
                            if (mesh.material.color.a == 1)
                                fadeInCue1.cueShouldFadeIn = false;
                            else
                                fadeInCue1.cueShouldFadeIn = false;  
                        }
                        else if (pillar.name.StartsWith("Pillar2_"))
                        {
                            if (mesh.material.color.a == 1)
                                fadeInCue2.cueShouldFadeIn = true;
                            else
                                fadeInCue2.cueShouldFadeIn = false;
                        }
                    }
                }
            }

            float xPos = 0;
            float ySize = 0;
            foreach (GameObject pillar in pillars)
            {
                if (pillar.name.StartsWith("Pillar0_"))
                {
                    xPos = pillar.transform.position.x;
                    ySize = pillar.transform.position.y;
                    break;
                }
            }

            float arenaSize = stateMachine._sceneController.scene.Size.x;
            Vector3 leftWallPosition = new Vector3(Math.Abs(xPos) * -1f, ySize+1, -1);
            Vector3 rightWallPosition = new Vector3(Math.Abs(xPos), ySize+1, -1);

            GameObject leftWall = Instantiate(trackWall, leftWallPosition, Quaternion.identity, stateMachine.transform);
            leftWall.name = "LeftWall";
            leftWall.transform.localScale = new Vector3(4, ySize*4 - 45f, arenaSize);
            leftWall.GetComponentInChildren<MeshRenderer>().material = stateMachine._sceneController.materials["grey"];


            GameObject rightWall = Instantiate(trackWall, rightWallPosition, Quaternion.identity, stateMachine.transform);
            rightWall.name = "RightWall";
            rightWall.transform.localScale = new Vector3(4, ySize*4 - 45f, arenaSize);
            rightWall.GetComponentInChildren<MeshRenderer>().material = stateMachine._sceneController.materials["grey"];

            // Black end cap: back wall + ceiling to hide pillars beyond the track end
            float endZ = arenaSize / 2f;
            float capWidth = Math.Abs(xPos) * 2f + 10f;

            // GameObject endWall = Instantiate(trackWall, new Vector3(0, ySize -1, endZ), Quaternion.identity, stateMachine.transform);
            // endWall.name = "EndWall";
            // endWall.transform.localScale = new Vector3(capWidth, ySize * 4, 4);
            // MeshRenderer endWallRenderer = endWall.GetComponentInChildren<MeshRenderer>();
            // endWallRenderer.material = stateMachine._sceneController.materials["grey"];
            // endWallRenderer.material.color = new Color(0, 0, 0, 1);
            // Collider endWallCollider = endWall.GetComponentInChildren<Collider>();
            // if (endWallCollider != null)
            // {
            //     endWallCollider.enabled = false;
            // }

            // Tree landmark outside left wall
            GameObject tree = Instantiate(Resources.Load<GameObject>("Tree Type4 03"), stateMachine.transform);
            tree.name = "TreeLandmark";
            tree.transform.position = new Vector3(Math.Abs(xPos) * -1f - 18f, 0, 220f);
            tree.transform.localScale = new Vector3(35f, 35f, 35f);
        }

    }
}
