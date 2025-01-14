using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0800/P0800_TrialStartLinearTrack")]

    public class P0800_TrialStartLinearTrack : FSMAction
    {
        public int cueIndicator = -1;
        public FadeScreen fadeScreen;
        public P0800_RewardConditionReached rewardConditionReached;
        public int firstRewardNum = 0;
        public int currentRewardStateID = 0;
        private float forwardGainDefault = -1;
        private int rewardFlip = 0;
        public override void Execute(BaseStateMachine stateMachine)
        {

            Vector3 newStartPosition = new Vector3(0, 0, -169f);
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
            {
                forwardGainDefault = stateMachine._playerMovement.ballForwardNormToCentimeter;
            }
            // manipulate the forward gain 
            if (stateMachine._sessionManager.trialVariablesDict.ContainsKey("GF"))
                stateMachine._playerMovement.ballForwardNormToCentimeter = forwardGainDefault * float.Parse(stateMachine._sessionManager.trialVariablesDict["GF"]);

            foreach (Transform child in stateMachine.transform)
            {
                if (!child.name.StartsWith("Pillar0_"))
                    child.gameObject.SetActive(true);
            }

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

            // if rewardFlip = 0, the near reward texture is whitedots and far reward texture is verticalstribes
            rewardFlip = 0;
            if (stateMachine._sessionManager.trialVariablesDict.ContainsKey("RF"))
                rewardFlip = int.Parse(stateMachine._sessionManager.trialVariablesDict["RF"]);

            // configure the cue
            foreach (Transform child in stateMachine.transform)
            {
                if (child.name.StartsWith("Pillar1_") || child.name.StartsWith("Pillar2_"))
                {
                    MeshRenderer[] meshRenderer = child.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer mesh in meshRenderer)
                    {
                        if (mesh.gameObject.name == "Cylinder")
                        {
                            if ((cueIndicator == 3 && rewardFlip == 0) || (cueIndicator == 4 && rewardFlip == 1))
                                mesh.material = stateMachine._sceneController.materials["whitedots"];
                            else if ((cueIndicator == 3 && rewardFlip == 1) || (cueIndicator == 4 && rewardFlip == 0))
                                mesh.material = stateMachine._sceneController.materials["verticalstribes"];
                            
                            mesh.material.color = new Color(1, 1, 1, 0);
                            float scaleOriginal = mesh.transform.localScale.y;
                            Debug.Log("Scale original: " + scaleOriginal);
                            mesh.material.mainTextureScale = new Vector2(scaleOriginal/25f*3f, scaleOriginal/25f);

                        }

                    }
                }
            }
      
            stateMachine._sessionManager.newTrial();
            stateMachine._sessionManager.currentRewardNum = 0;
            stateMachine._sessionManager.trialRunning = true;
            stateMachine._sessionManager.rewardSucked = false;


        }
    }
}
