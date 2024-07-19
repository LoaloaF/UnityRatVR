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
        public float enterRewardPosZ;
        public float endRewardPosZ;
        public bool trialSuccess = false;
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
            trialSuccess = false;


            foreach (Transform child in stateMachine.transform)
            {
                child.gameObject.SetActive(true);
            }


            GameObject[] pillars = GameObject.FindGameObjectsWithTag("Pillar");
            float randomValue = Random.Range(0f, 1f);

            if (randomValue > 0.5f)
            {
                cueIndicator = 2;
                enterRewardPosZ = 60f;
                endRewardPosZ = 80f;
                Debug.Log("Cue at far");
            }
            else
            {
                cueIndicator = 1;
                enterRewardPosZ = 20f;
                endRewardPosZ = 40f;
                Debug.Log("Cue at near");
            }

            foreach (Transform child in stateMachine.transform)
            {
                if (child.name == "ClueZone(Clone)")
                {
                    MeshRenderer[] meshRenderer = child.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer mesh in meshRenderer)
                    {
                        if (cueIndicator == 1)
                        {
                            mesh.material = stateMachine._sceneController.materials["testwall3"];
                        }
                        else if (cueIndicator == 2)
                        {
                            mesh.material = stateMachine._sceneController.materials["verticalstribes"];;
                        }
                    }
                }
            }
      
            stateMachine._sessionManager.newTrial();
            stateMachine._sessionManager.trialRunning = true;


        }
    }
}
