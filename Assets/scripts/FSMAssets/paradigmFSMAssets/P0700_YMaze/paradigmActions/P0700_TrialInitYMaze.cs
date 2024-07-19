using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0700/P0700_TrialInitYMaze")]

    public class P0700_TrialInitYMaze : FSMAction
    {
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

            GameObject[] pillars = GameObject.FindGameObjectsWithTag("Pillar");

            int pillarCount = 0;
            foreach (GameObject pillar in pillars)
            {
                if (pillar.name.StartsWith("Pillar1"))
                {
                    stateMachine._sceneController.scene.Pillars[pillarCount].IsReward = 0;

                    Collider pillarCollider = pillar.GetComponentInChildren<CapsuleCollider>();
                    pillarCollider.isTrigger = false;

                }
                pillarCount++;
            }
        }

    }
}
