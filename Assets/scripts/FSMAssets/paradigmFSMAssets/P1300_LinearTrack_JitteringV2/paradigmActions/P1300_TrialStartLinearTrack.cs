using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
namespace Experiment.ExperimentFSM
{
        [CreateAssetMenu(menuName = "FSM/Actions/P1300/P1300_TrialStartLinearTrack")]

    public class P1300_TrialStartLinearTrack : FSMAction
    {
        public int cueIndicator = -1;
        public FadeScreen fadeScreen;
        public P1300_RewardConditionReached rewardConditionReached;
        public int firstRewardNum = 0;
        public int currentRewardStateID = 0;
        private float forwardGainDefault = -1;
        private int[] last3Cues = { -1, -1, -1 };

        public float chosenCueDistance = 0f;
        public float chosenRewardDistance = 0f;
        public float[] cueProbabilites = { 0.33f, 0.33f, 0.33f};
        public float[] rewardProbabilites = { 0.33f, 0.33f, 0.33f};

        private static readonly int[] cueDetectionGroup    = { 1 };
        private static readonly int[] cueVisualGroup       = { 9 };
        private static readonly int[] rewardDetectionGroup = { 2 };
        private static readonly int[] rewardVisualGroup    = { 14 };
        private static readonly int[] cueFlankLeft         = { 101 };
        private static readonly int[] cueFlankRight        = { 201 };
        private static readonly int[] rewardFlankLeft      = { 102 };
        private static readonly int[] rewardFlankRight     = { 202 };

        public override void Execute(BaseStateMachine stateMachine)
        {
            float arenaHalfZ = 0.5f * stateMachine._sceneController.scene.BaseLength * stateMachine._sceneController.scene.Size.y;
            Vector3 newStartPosition = new Vector3(0, 0, -arenaHalfZ);
            stateMachine._playerMovement.TeleportRat(newStartPosition.x, newStartPosition.z, newStartPosition.y);

            stateMachine._sceneController.floor.SetActive(true);
            stateMachine._sceneController.wallZone.SetActive(true);
            stateMachine.validationSphereRenderer.enabled = false;
            stateMachine._sceneController.wallTop.SetActive(true);
            stateMachine._sceneController.wallBottom.SetActive(true);
            stateMachine._sceneController.wallLeft.SetActive(true);
            stateMachine._sceneController.wallRight.SetActive(true);
            firstRewardNum = 0;

            fadeScreen = FindObjectOfType<FadeScreen>();
            fadeScreen.isFadingIn = false;
            fadeScreen.isFadingOut = false;
            currentRewardStateID = 0;

            if (forwardGainDefault == -1)
                forwardGainDefault = stateMachine._playerMovement.ballForwardNormToCentimeter;

            if (stateMachine._sessionManager.trialVariablesDict.ContainsKey("GF"))
                stateMachine._playerMovement.ballForwardNormToCentimeter = forwardGainDefault * float.Parse(stateMachine._sessionManager.trialVariablesDict["GF"]);

            foreach (Transform child in stateMachine.transform)
            {
                if (!child.name.StartsWith("Pillar0_"))
                    child.gameObject.SetActive(true);
            }


            // float distance_param = float.Parse(stateMachine._sceneController.distanceCueReward);
            // cueRewardDistances = new float[] { 105f + distance_param, 140f + distance_param, 175f + distance_param };


            // Apply scenario offsets
            GameObject[] pillars = GameObject.FindGameObjectsWithTag("Pillar");
            ApplyScenario(pillars, stateMachine);

            foreach (GameObject pillar in pillars)                                                                                                                                                   
            {
                var pc = pillar.GetComponent<PillarCollision>();                                                                                                                                     
                if (pc != null)
                    pc.RecomputeColliderParameters();
            }                                                                                                                                                                                        

            float randomValue = Random.Range(0f, 1f);
            float trialPortion = 0.5f;
            if (stateMachine._sessionManager.trialVariablesDict.ContainsKey("P_C1"))
                trialPortion = float.Parse(stateMachine._sessionManager.trialVariablesDict["P_C1"]);

            if (randomValue > trialPortion)
            {
                cueIndicator = 1;
                Debug.Log("Cue indicator 1 is shown");
                stateMachine._sessionManager.trialVariablesDict["C"] = "1";
            }
            else
            {
                cueIndicator = 2;
                Debug.Log("Cue indicator 2 is shown");
                stateMachine._sessionManager.trialVariablesDict["C"] = "2";
            }

            if (last3Cues[0] == last3Cues[1] && last3Cues[1] == last3Cues[2] && last3Cues[0] != -1)
            {
                cueIndicator = (last3Cues[0] == 2) ? 1 : 2;
                stateMachine._sessionManager.trialVariablesDict["C"] = (cueIndicator == 2) ? "1" : "2";
                Debug.Log("Overriding cue to avoid repetition");
            }
            else
            {
                Debug.Log("Cue order ok, current last three cues: " + last3Cues[0] + " " + last3Cues[1] + " " + last3Cues[2]);
            }

            last3Cues[0] = last3Cues[1];
            last3Cues[1] = last3Cues[2];
            last3Cues[2] = cueIndicator;

            foreach (Transform child in stateMachine.transform)
            {
                if (child.name.StartsWith("Pillar1_") || child.name.StartsWith("Pillar2_") || child.name.StartsWith("Pillar101_") || child.name.StartsWith("Pillar201_") || child.name.StartsWith("Pillar102_") || child.name.StartsWith("Pillar202_"))
                {
                    MeshRenderer[] meshRenderer = child.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer mesh in meshRenderer)
                    {
                        if (mesh.gameObject.name == "Cylinder")
                        {

                            string cue1 = stateMachine._sceneController.cue1Texture;
                            string cue2 = stateMachine._sceneController.cue2Texture;

                            if (child.name.StartsWith("Pillar102_") || child.name.StartsWith("Pillar202_") || child.name.StartsWith("Pillar2_"))
                            {
                                mesh.material = stateMachine._sceneController.materials[cue1];
                            }
                            else
                            {
                                if (cueIndicator == 1)
                                    mesh.material = stateMachine._sceneController.materials[cue1];
                                else if (cueIndicator == 2)
                                    mesh.material = stateMachine._sceneController.materials[cue2];

                            }
                                mesh.material.color = new Color(1, 1, 1, 0);
                                float scaleOriginal = mesh.transform.localScale.y;
                                //Debug.Log("Scale original: " + scaleOriginal);
                                mesh.material.mainTextureScale = new Vector2(scaleOriginal / 25f * 3f, scaleOriginal / 25f);
                        }
                    }
                }
            }

            stateMachine._sessionManager.newTrial();
            stateMachine._sessionManager.currentRewardNum = 0;
            stateMachine._sessionManager.trialRunning = true;
            stateMachine._sessionManager.rewardSucked = false;
        }

        private void ApplyScenario(GameObject[] pillars, BaseStateMachine stateMachine)
        {
            float offsetCue     = float.Parse(stateMachine._sceneController.offsetCue);
            float offsetReward  = float.Parse(stateMachine._sceneController.offsetReward);
            float offsetVisible = float.Parse(stateMachine._sceneController.offsetVisible);
            float jsCue         = float.Parse(stateMachine._sceneController.jitterStrengthCue);
            float jsReward      = float.Parse(stateMachine._sceneController.jitterStrengthReward);

            float probabilityCueNear = float.Parse(stateMachine._sessionManager.trialVariablesDict["P_CN"]);
            float probabilityCueMedium = float.Parse(stateMachine._sessionManager.trialVariablesDict["P_CM"]);
            float probablityCueFar = float.Parse(stateMachine._sessionManager.trialVariablesDict["P_CF"]);

            float probabilityRewardNear = float.Parse(stateMachine._sessionManager.trialVariablesDict["P_RN"]);
            float probabiltyRewardMedium = float.Parse(stateMachine._sessionManager.trialVariablesDict["P_RM"]);
            float probabilityRewardFar = float.Parse(stateMachine._sessionManager.trialVariablesDict["P_RF"]);

            float arenaHalfZ = 0.5f * stateMachine._sceneController.scene.BaseLength * stateMachine._sceneController.scene.Size.y;
            float spawnZ = -arenaHalfZ;
            
            float cueFar = offsetCue + jsCue;
            float cueMiddle = offsetCue;
            float cueNear = offsetCue - jsCue;

            float rewardFar = offsetReward + jsReward;
            float rewardMiddle = offsetReward;
            float rewardNear = offsetReward - jsReward;

            float[] cueScenarios    = { cueNear, cueMiddle, cueFar };
            float[] rewardScenarios = { rewardNear, rewardMiddle, rewardFar };

            string[] cueScenarioLabels    = { "cueNear", "cueMedium", "cueFar" };
            string[] rewardScenarioLabels = { "rewardNear", "rewardMedium", "rewardFar" };

            cueProbabilites = new float[] { probabilityCueNear, probabilityCueMedium, probablityCueFar };
            rewardProbabilites = new float[] { probabilityRewardNear, probabiltyRewardMedium, probabilityRewardFar };

            int chosenCueIndex = ChooseIndex(cueProbabilites);
            int chosenRewardIndex = ChooseIndex(rewardProbabilites);

            chosenCueDistance    = cueScenarios[chosenCueIndex];
            chosenRewardDistance = rewardScenarios[chosenRewardIndex];

            stateMachine._sessionManager.trialVariablesDict["CD"] = cueScenarioLabels[chosenCueIndex];
            stateMachine._sessionManager.trialVariablesDict["RD"] = rewardScenarioLabels[chosenRewardIndex];

            float cueZ         = spawnZ + chosenCueDistance;
            float cueVisualZ   = cueZ - offsetVisible;
            float rewardZ      = spawnZ + chosenCueDistance + chosenRewardDistance;
            float rewardVisualZ = rewardZ - offsetVisible;

            foreach (int idx in cueDetectionGroup)
                MovePillarToZ(pillars, idx, cueZ);
            foreach (int idx in cueVisualGroup)
                MovePillarToZ(pillars, idx, cueVisualZ);
            foreach (int idx in rewardDetectionGroup)
                MovePillarToZ(pillars, idx, rewardZ);
            foreach (int idx in rewardVisualGroup)
                MovePillarToZ(pillars, idx, rewardVisualZ);

            Transform leftWallT  = stateMachine.transform.Find("LeftWall");
            Transform rightWallT = stateMachine.transform.Find("RightWall");
            float leftWallX  = leftWallT  != null ? leftWallT.position.x  : 0f;
            float rightWallX = rightWallT != null ? rightWallT.position.x : 0f;

            foreach (int idx in cueFlankLeft)
                MovePillarFlank(pillars, idx, leftWallX, cueZ, -1f);
            foreach (int idx in cueFlankRight)
                MovePillarFlank(pillars, idx, rightWallX, cueZ, 1f);
            foreach (int idx in rewardFlankLeft)
                MovePillarFlank(pillars, idx, leftWallX, rewardZ, -1f);
            foreach (int idx in rewardFlankRight)
                MovePillarFlank(pillars, idx, rightWallX, rewardZ, 1f);

            Debug.Log($"Scenario: chosenCue={chosenCueDistance:F0}, chosenReward={chosenRewardDistance:F0}, cueZ={cueZ:F0}, rewardZ={rewardZ:F0}");
        }

        private void MovePillarFlank(GameObject[] pillars, int pillarIdx, float wallX, float targetZ, float signX)
        {
            foreach (GameObject pillar in pillars)
            {
                if (pillar.name.StartsWith("Pillar" + pillarIdx + "_"))
                {
                    Transform cylinder = pillar.transform.Find("Cylinder");
                    float radius = cylinder != null ? cylinder.localScale.x / 2f : 0f;
                    pillar.transform.position = new Vector3(wallX + signX * radius - signX*2.1f, pillar.transform.position.y, targetZ); //-2.1f so that the pillar just sticks right out of the wall
                }
            }
        }

        private void MovePillarToZ(GameObject[] pillars, int pillarIdx, float targetZ)
        {
            foreach (GameObject pillar in pillars)
            {
                if (pillar.name.StartsWith("Pillar" + pillarIdx + "_"))
                {
                    Vector3 pos = pillar.transform.position;
                    pillar.transform.position = new Vector3(pos.x, pos.y, targetZ);
                }
            }
        }

        private int ChooseIndex(float[] probabilites)
        { 
            float randomValue = Random.Range(0f, 1f);

            for (int i = 0; i < probabilites.Length; i++)
            {
                if (randomValue < probabilites[i])
                {
                    // Debug.Log("Chosen index: " + i);
                    return i;
                }
                else
                    randomValue -= probabilites[i];
        
            }
           Debug.LogWarning("Probabilities do not sum to 1, returning random index by default");
            return Random.Range(0, probabilites.Length);      
        }  

    }
}