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
    [CreateAssetMenu(menuName = "FSM/Actions/P0800/P0800_TrialInitLinearTrack")]

    public class P0800_TrialInitLinearTrack : FSMAction
    {
        public GameObject trackWall;
        public P0800_FadeInCue fadeInCue1;
        public P0800_FadeInCue fadeInCue2;

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


            stateMachine._playerMovement.ballSidewaysNormToCentimeter = 0f;
            stateMachine._playerMovement.ballRotatationNormToCentimeter = 0f;
            stateMachine._playerMovement.zOnlyMovePositive = true;

            GameObject[] pillars = GameObject.FindGameObjectsWithTag("Pillar");
            Debug.Log("Pillar count: " + pillars.Length);
            foreach (GameObject pillar in pillars)
            {
                if (pillar.name.StartsWith("Pillar11"))
                {
                    pillar.SetActive(false);
                }
                else if (pillar.name.StartsWith("Pillar3") || pillar.name.StartsWith("Pillar4"))
                {
                    MeshRenderer[] meshRenderer = pillar.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer mesh in meshRenderer)
                    {
                        if (mesh.gameObject.name == "Cylinder")
                        {
                            mesh.material.mainTextureScale = new Vector2(3.37f, 3.37f);
                        }

                    }
                }
                else if ((pillar.name.StartsWith("Pillar1") && !pillar.name.StartsWith("Pillar11")) || pillar.name.StartsWith("Pillar2"))
                {
                    MeshRenderer[] meshRenderer = pillar.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer mesh in meshRenderer)
                    {
                        if ((pillar.name.StartsWith("Pillar1") && !pillar.name.StartsWith("Pillar11")))
                        {
                            if (mesh.material.color.a == 1)
                                fadeInCue1.cueShouldFadeIn = true;
                            else
                                fadeInCue1.cueShouldFadeIn = false;
                        }
                        else if (pillar.name.StartsWith("Pillar2"))
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
                if (pillar.name.StartsWith("Pillar11"))
                {
                    xPos = pillar.transform.position.x;
                    ySize = pillar.transform.position.y;
                    break;
                }
            }

            float arenaSize = stateMachine._sceneController.scene.Size.x;
            Vector3 leftWallPosition = new Vector3(Math.Abs(xPos) * -1f, ySize+1, 0);
            Vector3 rightWallPosition = new Vector3(Math.Abs(xPos), ySize+1, 0);

            GameObject leftWall = Instantiate(trackWall, leftWallPosition, Quaternion.identity, stateMachine.transform);
            leftWall.name = "LeftWall";
            leftWall.transform.localScale = new Vector3(4, ySize*2, arenaSize);
            leftWall.GetComponentInChildren<MeshRenderer>().material = stateMachine._sceneController.materials["grey"];


            GameObject rightWall = Instantiate(trackWall, rightWallPosition, Quaternion.identity, stateMachine.transform);
            rightWall.name = "RightWall";
            rightWall.transform.localScale = new Vector3(4, ySize*2, arenaSize);
            rightWall.GetComponentInChildren<MeshRenderer>().material = stateMachine._sceneController.materials["grey"];

        }

    }
}
