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
            ValidateJitteringConstraints(stateMachine);

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
            pillarCylinderMeshes.Clear();

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
 

            foreach (GameObject pillar in pillars)
            {
                if (pillar.name.StartsWith("Pillar0_"))
                {
                    pillar.SetActive(false);
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
            //changed Size.x to Size.y, for the length of the walls 
            float arenaSize = stateMachine._sceneController.scene.Size.y;
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


            // Offset ceiling for the tree
            stateMachine._sceneController.ceiling.transform.position = new Vector3(0, stateMachine._sceneController.ceiling.transform.position.y + 180f, 0);


            // Tree landmark outside left wall
            GameObject tree = Instantiate(Resources.Load<GameObject>("Tree Type4 03"), stateMachine.transform);
            tree.name = "TreeLandmark";
            tree.transform.position = new Vector3(Math.Abs(xPos) * -1f - 18f, 0, 220f);
            tree.transform.localScale = new Vector3(35f, 35f, 35f);
        }

    private void ValidateJitteringConstraints(BaseStateMachine stateMachine)
    {
        float offsetCue     = float.Parse(stateMachine._sceneController.offsetCue);
        float offsetReward  = float.Parse(stateMachine._sceneController.offsetReward);
        float offsetVisible = float.Parse(stateMachine._sceneController.offsetVisible);
        float jsCue         = float.Parse(stateMachine._sceneController.jitterStrengthCue);
        float jsReward      = float.Parse(stateMachine._sceneController.jitterStrengthReward);
        float arenaLength   = stateMachine._sceneController.scene.BaseLength * stateMachine._sceneController.scene.Size.y;

        PillarData rewardPillar = stateMachine._sceneController.scene.Pillars.Find(p => p.UID.StartsWith("2_"));
        PillarData cuePillar = stateMachine._sceneController.scene.Pillars.Find(p => p.UID.StartsWith("1_"));
        float rewardRadius = rewardPillar != null ? rewardPillar.RewardRadius : 0f;
        float cueRadius = cuePillar != null ? cuePillar.Radius : 0f;
        if (offsetVisible <= rewardRadius || offsetVisible <= cueRadius)
            throw new Exception($"P1300 Constraint 1 violated: visibleOffset ({offsetVisible}) must be > rewardRadius ({rewardRadius}) and > cueRadius ({cueRadius})");
        if (offsetCue <= offsetVisible + jsCue)
            throw new Exception($"P1300 Constraint 2 violated: cueOffset ({offsetCue}) must be > visibleOffset ({offsetVisible}) + cueJitter ({jsCue})");
        if (offsetReward <= offsetVisible + jsReward)
            throw new Exception($"P1300 Constraint 3 violated: rewardOffset ({offsetReward}) must be > visibleOffset ({offsetVisible}) + rewardJitter ({jsReward})");
        if (offsetCue + offsetReward + jsCue + jsReward + 2f * rewardRadius >= arenaLength)
            throw new Exception($"P1300 Constraint 4 violated: trial size ({offsetCue + offsetReward + jsCue + jsReward + 2f * rewardRadius}) must be < arenaLength ({arenaLength})");
        if (offsetReward <= rewardRadius + cueRadius + jsReward)
            throw new Exception($"P1300 Constraint 5 violated: rewardOffset ({offsetReward}) must be > rewardRadius ({rewardRadius}) + cueRadius ({cueRadius}) + rewardJitter ({jsReward})");
    }
    }
}
