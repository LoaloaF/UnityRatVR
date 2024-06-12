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
            landmark.transform.localScale = new Vector3(landmark.transform.localScale.x * stateMachine._sceneController.scene.BaseLength, 
                                                        landmark.transform.localScale.y * stateMachine._sceneController.scene.BaseLength, 
                                                        landmark.transform.localScale.z * stateMachine._sceneController.scene.BaseLength);
            landmark.transform.position = new Vector3(0, 56, 34*stateMachine._sceneController.scene.BaseLength);

            landmark = Instantiate(LandmarkObject);
            landmark.transform.localScale = new Vector3(landmark.transform.localScale.x * stateMachine._sceneController.scene.BaseLength, 
                                                        landmark.transform.localScale.y * stateMachine._sceneController.scene.BaseLength, 
                                                        landmark.transform.localScale.z * stateMachine._sceneController.scene.BaseLength);
            landmark.transform.position = new Vector3(0, 56, -34*stateMachine._sceneController.scene.BaseLength);

            landmark = Instantiate(LandmarkObject);
            landmark.transform.localScale = new Vector3(landmark.transform.localScale.x * stateMachine._sceneController.scene.BaseLength, 
                                                        landmark.transform.localScale.y * stateMachine._sceneController.scene.BaseLength, 
                                                        landmark.transform.localScale.z * stateMachine._sceneController.scene.BaseLength);
            landmark.transform.position = new Vector3(30*stateMachine._sceneController.scene.BaseLength, 56, 0);

            landmark = Instantiate(LandmarkObject);
            landmark.transform.localScale = new Vector3(landmark.transform.localScale.x * stateMachine._sceneController.scene.BaseLength, 
                                                        landmark.transform.localScale.y * stateMachine._sceneController.scene.BaseLength, 
                                                        landmark.transform.localScale.z * stateMachine._sceneController.scene.BaseLength);
            landmark.transform.position = new Vector3(-30*stateMachine._sceneController.scene.BaseLength, 56, 0);




            stateMachine._sceneController.wallTop.transform.position = new Vector3(stateMachine._sceneController.wallTop.transform.position.x * scaleFactor, 
                                                                                   stateMachine._sceneController.wallTop.transform.position.y, 
                                                                                   stateMachine._sceneController.wallTop.transform.position.z);
            
            stateMachine._sceneController.wallBottom.transform.position = new Vector3(stateMachine._sceneController.wallBottom.transform.position.x * scaleFactor, 
                                                                                      stateMachine._sceneController.wallBottom.transform.position.y, 
                                                                                      stateMachine._sceneController.wallBottom.transform.position.z);
            
            stateMachine._sceneController.wallLeft.transform.position = new Vector3(stateMachine._sceneController.wallLeft.transform.position.x, 
                                                                                    stateMachine._sceneController.wallLeft.transform.position.y, 
                                                                                    stateMachine._sceneController.wallLeft.transform.position.z * scaleFactor);
            
            stateMachine._sceneController.wallRight.transform.position = new Vector3(stateMachine._sceneController.wallRight.transform.position.x, 
                                                                                     stateMachine._sceneController.wallRight.transform.position.y, 
                                                                                     stateMachine._sceneController.wallRight.transform.position.z * scaleFactor);
                                                    
            stateMachine._sceneController.wallTop.transform.localScale = new Vector3(stateMachine._sceneController.wallTop.transform.localScale.x * scaleFactor, 
                                                                                stateMachine._sceneController.wallTop.transform.localScale.y, 
                                                                                stateMachine._sceneController.wallTop.transform.localScale.z);
            
            stateMachine._sceneController.wallBottom.transform.localScale = new Vector3(stateMachine._sceneController.wallBottom.transform.localScale.x * scaleFactor, 
                                                                                   stateMachine._sceneController.wallBottom.transform.localScale.y, 
                                                                                   stateMachine._sceneController.wallBottom.transform.localScale.z);
            
            stateMachine._sceneController.wallLeft.transform.localScale = new Vector3(stateMachine._sceneController.wallLeft.transform.localScale.x * scaleFactor, 
                                                                                 stateMachine._sceneController.wallLeft.transform.localScale.y, 
                                                                                 stateMachine._sceneController.wallLeft.transform.localScale.z);
            
            stateMachine._sceneController.wallRight.transform.localScale = new Vector3(stateMachine._sceneController.wallRight.transform.localScale.x * scaleFactor, 
                                                                                  stateMachine._sceneController.wallRight.transform.localScale.y, 
                                                                                  stateMachine._sceneController.wallRight.transform.localScale.z);

            stateMachine._sceneController.ceiling.transform.localScale = new Vector3(stateMachine._sceneController.ceiling.transform.localScale.x * scaleFactor, 
                                                                               stateMachine._sceneController.ceiling.transform.localScale.y, 
                                                                               stateMachine._sceneController.ceiling.transform.localScale.z * scaleFactor);                                                  


            stateMachine._sceneController.floor.SetActive(true);
            stateMachine._sceneController.wallZone.SetActive(true);
            stateMachine.validationSphereRenderer.enabled = false;
            stateMachine._sceneController.wallTop.SetActive(true);
            stateMachine._sceneController.wallBottom.SetActive(true);
            stateMachine._sceneController.wallLeft.SetActive(true);
            stateMachine._sceneController.wallRight.SetActive(true);
            stateMachine._playerMovement.EnableMovement();
            Vector3 newStartPosition = new Vector3(0, 0, 0);
            stateMachine._playerMovement.TeleportRat(newStartPosition.x, newStartPosition.z, newStartPosition.y);
        }

    }
}
