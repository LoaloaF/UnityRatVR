using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0300/P0300_TrialInitHexagon")]

    public class P0300_TrialInitHexagon : FSMAction
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
            P0300_LandmarkFollowPlayer landmarkFollowPlayer = landmark.GetComponent<P0300_LandmarkFollowPlayer>();
            landmarkFollowPlayer.initLandmarkPosition = landmark.transform.position;
            landmarkFollowPlayer.finishInit = true;
            landmark.GetComponentInChildren<MeshRenderer>().material = stateMachine._sceneController.materials["testwall6"];


            landmark = Instantiate(LandmarkObject);
            landmark.name = "LandmarkY";
            landmark.transform.localScale = new Vector3(landmark.transform.localScale.x * stateMachine._sceneController.scene.BaseLength * scaleFactor, 
                                                        landmark.transform.localScale.y * stateMachine._sceneController.scene.BaseLength * scaleFactor, 
                                                        landmark.transform.localScale.z * stateMachine._sceneController.scene.BaseLength * scaleFactor);
            landmark.transform.position = new Vector3(0, 56, -34*stateMachine._sceneController.scene.BaseLength * scaleFactor);
            landmarkFollowPlayer = landmark.GetComponent<P0300_LandmarkFollowPlayer>();
            landmarkFollowPlayer.initLandmarkPosition = landmark.transform.position;
            landmarkFollowPlayer.finishInit = true;
                    landmark.GetComponentInChildren<MeshRenderer>().material = stateMachine._sceneController.materials["testwall7"];


            landmark = Instantiate(LandmarkObject);
            landmark.name = "LandmarkX";
            landmark.transform.localScale = new Vector3(landmark.transform.localScale.x * stateMachine._sceneController.scene.BaseLength * scaleFactor, 
                                                        landmark.transform.localScale.y * stateMachine._sceneController.scene.BaseLength * scaleFactor, 
                                                        landmark.transform.localScale.z * stateMachine._sceneController.scene.BaseLength * scaleFactor);
            landmark.transform.position = new Vector3(30*stateMachine._sceneController.scene.BaseLength * scaleFactor, 56, 0);
            landmarkFollowPlayer = landmark.GetComponent<P0300_LandmarkFollowPlayer>();
            landmarkFollowPlayer.initLandmarkPosition = landmark.transform.position;
            landmarkFollowPlayer.finishInit = true;
            landmark.GetComponentInChildren<MeshRenderer>().material = stateMachine._sceneController.materials["testwall8"];


            landmark = Instantiate(LandmarkObject);
            landmark.name = "LandmarkX";
            landmark.transform.localScale = new Vector3(landmark.transform.localScale.x * stateMachine._sceneController.scene.BaseLength * scaleFactor, 
                                                        landmark.transform.localScale.y * stateMachine._sceneController.scene.BaseLength * scaleFactor, 
                                                        landmark.transform.localScale.z * stateMachine._sceneController.scene.BaseLength * scaleFactor);
            landmark.transform.position = new Vector3(-30*stateMachine._sceneController.scene.BaseLength * scaleFactor, 56, 0);
            landmarkFollowPlayer = landmark.GetComponent<P0300_LandmarkFollowPlayer>();
            landmarkFollowPlayer.initLandmarkPosition = landmark.transform.position;
            landmarkFollowPlayer.finishInit = true;
            landmark.GetComponentInChildren<MeshRenderer>().material = stateMachine._sceneController.materials["testwall3"];



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
