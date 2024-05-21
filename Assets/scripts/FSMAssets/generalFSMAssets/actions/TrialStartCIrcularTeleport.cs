using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/TrialStartAction")]

    public class TrialStartAction : FSMAction
    {

        private float pillarTransparency = 1;
        private float pillarIsRewarded = 1;
        private float pillarIsPunishment = 0;
        private float pillarDist;
        private float pillarAngle;
        private int pillarNumber;

        private string pillarTransparencyString;
        private string pillarIsRewardedString;
        private string pillarIsPunishmentString;
        private string packValues;

        public override void Execute(BaseStateMachine stateMachine)
        {
            stateMachine._sceneController.floor.SetActive(true);
            stateMachine._sceneController.wallZone.SetActive(true);

            // get the first child (the only pillar)
            Transform child = stateMachine.transform.GetChild(0);
            Vector3 pillarPosition = child.position;

            // rest the collision detection
            child.GetComponentInChildren<PillarCollision>().PlayerDetected = false;
            
            // set the reward and punishment value
            pillarIsRewarded = 1;
            pillarIsPunishment = 0;
            
            // setup the transparency of the pillar
            pillarTransparency = GenerateRandomValue(stateMachine._sessionManager.pillarTransparencyMin, 
                                                     stateMachine._sessionManager.pillarTransparencyMax);
            Debug.Log("pillarTransparency: " + pillarTransparency);
            Color originalColor = child.GetComponentInChildren<MeshRenderer>().material.color;
            child.GetComponentInChildren<MeshRenderer>().material.color = new Color(originalColor.r, 
                                                                                    originalColor.g, 
                                                                                    originalColor.b, 
                                                                                    pillarTransparency);


            // Teleport the rat to a new start position

            Debug.Log("nextTrialEndTeleportCenterDist: " + stateMachine._sessionManager.nextTrialEndTeleportCenterDist);
            Vector3 newStartPosition = samplenewStartPosition(pillarPosition, 
                                                              stateMachine._sessionManager.nextTrialEndTeleportCenterDist,
                                                              stateMachine._sessionManager.nextTrialEndTeleportCenterAngle);
            stateMachine._playerMovement.TeleportRat(newStartPosition.x, newStartPosition.z, newStartPosition.y);

            
            // prepare the scene and playermovement for the next session
            stateMachine.validationSphereRenderer.enabled = false;
            stateMachine._playerMovement.EnableMovement();


            // Prepare and log the trial
            pillarDist = stateMachine._sessionManager.nextTrialEndTeleportCenterDist;
            pillarAngle = stateMachine._sessionManager.nextTrialEndTeleportCenterAngle;

            pillarTransparencyString = pillarTransparency.ToString();
            pillarIsRewardedString = pillarIsRewarded.ToString();
            pillarIsPunishmentString = pillarIsPunishment.ToString();

            packValues = $"PD:{pillarDist},PA:{pillarAngle},P1T:{pillarTransparencyString},P1R:{pillarIsRewardedString},P1N:{pillarIsPunishmentString}";
            stateMachine._sessionManager.logNewTrial(packValues);
            stateMachine._sessionManager.trialRunning = true;

        }

        public Vector3 samplenewStartPosition(Vector3 center, float radius, float orientation=0)
        {
            // Generate a random angle in radians
            float angle = Random.Range(0, 2 * Mathf.PI);

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
