using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/TrialStartCircularTeleport")]

    public class TrialStartCircularTeleport : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
            stateMachine._sceneController.floor.SetActive(true);
            stateMachine._sceneController.wallZone.SetActive(true);

            // get the first child (the only pillar)
            Transform child = stateMachine.transform.GetChild(0);
            Vector3 pillarPosition = child.position;

            // rest the collision detection
            child.GetComponentInChildren<PillarCollision>().PlayerDetected = false;
            
            // setup the pillar transparency to 1
            Color originalColor = child.GetComponentInChildren<MeshRenderer>().material.color;
            child.GetComponentInChildren<MeshRenderer>().material.color = new Color(originalColor.r, 
                                                                                    originalColor.g, 
                                                                                    originalColor.b, 
                                                                                    1);

            // Teleport the rat to a new start position
            Vector3 newStartPosition = samplenewStartPosition(pillarPosition, 
                                                              stateMachine._sessionManager.nextTrialEndTeleportCenterDist,
                                                              stateMachine._sessionManager.nextTrialEndTeleportCenterAngle);
            stateMachine._playerMovement.TeleportRat(newStartPosition.x, newStartPosition.z, newStartPosition.y);

            
            // prepare the scene and playermovement for the next session
            stateMachine.validationSphereRenderer.enabled = false;
            stateMachine._sceneController.wallTop.SetActive(true);
            stateMachine._sceneController.wallBottom.SetActive(true);
            stateMachine._sceneController.wallLeft.SetActive(true);
            stateMachine._sceneController.wallRight.SetActive(true);
            stateMachine._playerMovement.EnableMovement();


            stateMachine._sessionManager.newTrial();
            stateMachine._sessionManager.trialRunning = true;

        }

        public Vector3 samplenewStartPosition(Vector3 center, float radius, float orientation=0)
        {
            // Generate a random angle in radians
            float angle = Random.Range(0, 2 * Mathf.PI);
            Debug.Log("Angle " + angle);


            // default to inverted anlge (pointing towrads center)
            // orientation = (angle+Mathf.PI) * Mathf.Rad2Deg;
            orientation += (angle) * Mathf.Rad2Deg - 180; 

            // Calculate the x and z coordinates
            float x = radius * Mathf.Sin(angle);
            float z = radius * Mathf.Cos(angle);
            Debug.Log("x: "+x);
            Debug.Log("z: "+z);

            // Create the new point
            Vector3 point = new Vector3(center.x + x, orientation, center.z + z);
            return point;
        }


        public float GenerateRandomValue(float _minVal, float _maxVal)
        {
            return Random.Range(_minVal, _maxVal);
        }
    }
}
