using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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
        private int rewardFlip = 0;
        private int[] last3Cues = new int[3] { -1, -1, -1 };

        // Scenarios: 3 cue positions x 3 cue-reward distances = 9 scenarios
        private static readonly float[] cueOffsets = { -60f, 0f, 60f };
        private static readonly float[] cueRewardDistances = { 120f, 150f, 180f };
        public float cueOffset = 0f;
        public float rewardOffset = 0f;

        public float Distance = 0f; // for logging which scenario was used, can be set from outside if needed
        private Dictionary<string, float> originalZPositions = new Dictionary<string, float>();
        private bool positionsInitialized = false;

        private static readonly int[] cueGroup = { 2, 9, 10, 11 };
        private static readonly int[] rewardGroup = { 3, 4, 12, 13, 14, 15, 104 };

        public override void Execute(BaseStateMachine stateMachine)
        {
            Vector3 newStartPosition = new Vector3(0, 0, -250f);
            stateMachine._playerMovement.TeleportRat(newStartPosition.x, newStartPosition.z, newStartPosition.y);

            stateMachine._sceneController.floor.SetActive(true);
            stateMachine._sceneController.wallZone.SetActive(true);
            stateMachine.validationSphereRenderer.enabled = false;
            stateMachine._sceneController.wallTop.SetActive(true);
            stateMachine._sceneController.wallBottom.SetActive(true);
            stateMachine._sceneController.wallLeft.SetActive(true);
            stateMachine._sceneController.wallRight.SetActive(true);
            rewardConditionReached.timer = 0;
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

            // Apply scenario offsets
            GameObject[] pillars = GameObject.FindGameObjectsWithTag("Pillar");
            ApplyScenario(pillars);

            float randomValue = Random.Range(0f, 1f);
            float trialPortion = 0.5f;
            if (stateMachine._sessionManager.trialVariablesDict.ContainsKey("NP"))
                trialPortion = float.Parse(stateMachine._sessionManager.trialVariablesDict["NP"]);

            if (randomValue > trialPortion)
            {
                cueIndicator = 4;
                Debug.Log("Cue at far");
                stateMachine._sessionManager.trialVariablesDict["C"] = "2";
            }
            else
            {
                cueIndicator = 3;
                Debug.Log("Cue at near");
                stateMachine._sessionManager.trialVariablesDict["C"] = "1";
            }

            if (last3Cues[0] == last3Cues[1] && last3Cues[1] == last3Cues[2] && last3Cues[0] != -1)
            {
                cueIndicator = (last3Cues[0] == 3) ? 4 : 3;
                stateMachine._sessionManager.trialVariablesDict["C"] = (cueIndicator == 3) ? "1" : "2";
                Debug.Log("Overriding cue to avoid repetition");
            }
            else
            {
                Debug.Log("Cue order ok, current last three cues: " + last3Cues[0] + " " + last3Cues[1] + " " + last3Cues[2]);
            }

            last3Cues[0] = last3Cues[1];
            last3Cues[1] = last3Cues[2];
            last3Cues[2] = cueIndicator;

            rewardFlip = 0;
            if (stateMachine._sessionManager.trialVariablesDict.ContainsKey("RF"))
                rewardFlip = int.Parse(stateMachine._sessionManager.trialVariablesDict["RF"]);

            foreach (Transform child in stateMachine.transform)
            {
                if (child.name.StartsWith("Pillar1_") || child.name.StartsWith("Pillar2_") || child.name.StartsWith("Pillar104_"))
                {
                    MeshRenderer[] meshRenderer = child.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer mesh in meshRenderer)
                    {
                        if (mesh.gameObject.name == "Cylinder")
                        {

                            // Check if it's Pillar104_ and always set it to whitedots
                            if (child.name.StartsWith("Pillar104_"))
                            {
                                mesh.material = stateMachine._sceneController.materials["whitedots"];
                            }
                            else
                            {
                                if ((cueIndicator == 3 && rewardFlip == 0) || (cueIndicator == 4 && rewardFlip == 1))
                                    mesh.material = stateMachine._sceneController.materials["whitedots"];
                                else if ((cueIndicator == 3 && rewardFlip == 1) || (cueIndicator == 4 && rewardFlip == 0))
                                    mesh.material = stateMachine._sceneController.materials["verticalstribes"];
                            }
                                mesh.material.color = new Color(1, 1, 1, 0);
                                float scaleOriginal = mesh.transform.localScale.y;
                                Debug.Log("Scale original: " + scaleOriginal);
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

    private void ApplyScenario(GameObject[] pillars)
    {
        if (originalZPositions.Count == 0)
            positionsInitialized = false;

        if (!positionsInitialized)
        {
            foreach (int idx in cueGroup.Concat(rewardGroup).Concat(new[] { 8 }))
            {
                foreach (GameObject pillar in pillars)
                {
                    if (pillar.name.StartsWith("Pillar" + idx + "_"))
                    {
                        originalZPositions[idx.ToString()] = pillar.transform.position.z;
                        break;
                    }
                }
            }
            positionsInitialized = true;
        }

        // Pick random scenario: 1 of 3 cue positions x 1 of 3 distances
        float chosenCueOffset = cueOffsets[Random.Range(0, cueOffsets.Length)];
        float chosenDistance = cueRewardDistances[Random.Range(0, cueRewardDistances.Length)];

        // Cue offset is relative to Pillar 10's original position
        cueOffset = chosenCueOffset;
        Distance = chosenDistance;
            // Reward offset: place reward at (cue position + distance) relative to Pillar 4's original position
        // trying out new logic where we take pillar 8 as the reference point
        float cueNewZ = originalZPositions["10"] + chosenCueOffset;
        float rewardOriginalZ = originalZPositions["4"];
        rewardOffset = (cueNewZ + chosenDistance) - rewardOriginalZ;

        Debug.Log($"Scenario: cueOffset={cueOffset:F0}, distance={chosenDistance:F0}, rewardOffset={rewardOffset:F0}");

        foreach (int idx in cueGroup)
            MovePillar(pillars, idx, cueOffset);
        foreach (int idx in rewardGroup)
            MovePillar(pillars, idx, rewardOffset);
    }

    private void MovePillar(GameObject[] pillars, int pillarIdx, float zOffset)
    {
        string key = pillarIdx.ToString();
        if (!originalZPositions.ContainsKey(key)) return;
        float origZ = originalZPositions[key];

        foreach (GameObject pillar in pillars)
        {
            if (pillar.name.StartsWith("Pillar" + pillarIdx + "_"))
            {
                Vector3 pos = pillar.transform.position;
                pillar.transform.position = new Vector3(pos.x, pos.y, origZ + zOffset);
            }
        }
    }
    }
}