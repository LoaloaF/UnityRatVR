using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/TrialInitHexagon")]

    public class TrialInitHexagon : FSMAction
    {
        [SerializeField] private GameObject LandmarkObject;

        public override void Execute(BaseStateMachine stateMachine)
        {

            int scaleFactor = 10;

            
            GameObject landmark = Instantiate(LandmarkObject);
            landmark.name = "LandmarkY";
            landmark.transform.localScale = new Vector3(landmark.transform.localScale.x * stateMachine._sceneController.scene.BaseLength * scaleFactor, 
                                                        landmark.transform.localScale.y * stateMachine._sceneController.scene.BaseLength * scaleFactor, 
                                                        landmark.transform.localScale.z * stateMachine._sceneController.scene.BaseLength * scaleFactor);
            landmark.transform.position = new Vector3(0, 56, 34*stateMachine._sceneController.scene.BaseLength * scaleFactor);
            LandmarkFollowPlayer landmarkFollowPlayer = landmark.GetComponent<LandmarkFollowPlayer>();
            landmarkFollowPlayer.initLandmarkPosition = landmark.transform.position;
            landmarkFollowPlayer.finishInit = true;

            landmark = Instantiate(LandmarkObject);
            landmark.name = "LandmarkY";
            landmark.transform.localScale = new Vector3(landmark.transform.localScale.x * stateMachine._sceneController.scene.BaseLength * scaleFactor, 
                                                        landmark.transform.localScale.y * stateMachine._sceneController.scene.BaseLength * scaleFactor, 
                                                        landmark.transform.localScale.z * stateMachine._sceneController.scene.BaseLength * scaleFactor);
            landmark.transform.position = new Vector3(0, 56, -34*stateMachine._sceneController.scene.BaseLength * scaleFactor);
            landmarkFollowPlayer = landmark.GetComponent<LandmarkFollowPlayer>();
            landmarkFollowPlayer.initLandmarkPosition = landmark.transform.position;
            landmarkFollowPlayer.finishInit = true;

            landmark = Instantiate(LandmarkObject);
            landmark.name = "LandmarkX";
            landmark.transform.localScale = new Vector3(landmark.transform.localScale.x * stateMachine._sceneController.scene.BaseLength * scaleFactor, 
                                                        landmark.transform.localScale.y * stateMachine._sceneController.scene.BaseLength * scaleFactor, 
                                                        landmark.transform.localScale.z * stateMachine._sceneController.scene.BaseLength * scaleFactor);
            landmark.transform.position = new Vector3(30*stateMachine._sceneController.scene.BaseLength * scaleFactor, 56, 0);
            landmarkFollowPlayer = landmark.GetComponent<LandmarkFollowPlayer>();
            landmarkFollowPlayer.initLandmarkPosition = landmark.transform.position;
            landmarkFollowPlayer.finishInit = true;

            landmark = Instantiate(LandmarkObject);
            landmark.name = "LandmarkX";
            landmark.transform.localScale = new Vector3(landmark.transform.localScale.x * stateMachine._sceneController.scene.BaseLength * scaleFactor, 
                                                        landmark.transform.localScale.y * stateMachine._sceneController.scene.BaseLength * scaleFactor, 
                                                        landmark.transform.localScale.z * stateMachine._sceneController.scene.BaseLength * scaleFactor);
            landmark.transform.position = new Vector3(-30*stateMachine._sceneController.scene.BaseLength * scaleFactor, 56, 0);
            landmarkFollowPlayer = landmark.GetComponent<LandmarkFollowPlayer>();
            landmarkFollowPlayer.initLandmarkPosition = landmark.transform.position;
            landmarkFollowPlayer.finishInit = true;



            stateMachine._sceneController.floor.SetActive(true);
            stateMachine._sceneController.wallZone.SetActive(true);
            stateMachine.validationSphereRenderer.enabled = false;
            stateMachine._sceneController.ceiling.SetActive(false);
            stateMachine._sceneController.wallTop.SetActive(false);
            stateMachine._sceneController.wallBottom.SetActive(false);
            stateMachine._sceneController.wallLeft.SetActive(false);
            stateMachine._sceneController.wallRight.SetActive(false);
            stateMachine._playerMovement.EnableMovement();
            Vector3 newStartPosition = new Vector3(0, 0, 0);
            stateMachine._playerMovement.TeleportRat(newStartPosition.x, newStartPosition.z, newStartPosition.y);
        }

    }
}
