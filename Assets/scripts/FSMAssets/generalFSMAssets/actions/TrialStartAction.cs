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
        public override void Execute(BaseStateMachine stateMachine)
        {
            // this is the position in excel coordicates, not unity coordinates
            // Debug.Log(stateMachine._sceneController.scene.Pillars[0].position);
            int pillarNum = stateMachine.transform.childCount;
            Transform child = stateMachine.transform.GetChild(0);
            child.GetComponentInChildren<PillarCollision>().PlayerDetected = false;

            Vector3 pillarPosition = child.position;
            Debug.Log("nextTrialEndTeleportCenterDist: "+stateMachine._sessionManager.nextTrialEndTeleportCenterDist);

            Vector3 newStartPosition = samplenewStartPosition(pillarPosition, 
                                                    stateMachine._sessionManager.nextTrialEndTeleportCenterDist,
                                                    stateMachine._sessionManager.nextTrialEndTeleportCenterAngle);

            // TEST create a cuvbe object at newStartPosition
            // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // cube.transform.position = newStartPosition;
            
            stateMachine.validationSphereRenderer.enabled = false;
            stateMachine._playerMovement.EnableMovement();
            stateMachine._playerMovement.TeleportRat(newStartPosition.x, newStartPosition.z, newStartPosition.y);


            float pillarDist = stateMachine._sessionManager.nextTrialEndTeleportCenterDist;
            float pillarAngle = stateMachine._sessionManager.nextTrialEndTeleportCenterAngle;
            // have this for every pillar
            string pillarTransparency = "1";
            string pillarIsRewarded = "1";
            string pillarPunishment = "1";


            string packValues = $"PD:{pillarDist},PA:{pillarAngle},P1T:{pillarTransparency},P1R:{pillarIsRewarded},P1N:{pillarPunishment}";
            stateMachine._sessionManager.logNewTrial(packValues);
        }

        public Vector3 samplenewStartPosition(Vector3 center, float radius, float orientation=0)
        {
            // Generate a random angle in radians
            float angle = Random.Range(0, 2 * Mathf.PI);
            Debug.Log("angle: "+angle);

            // default to inverted anlge (pointing towrads center)
            // orientation = (angle+Mathf.PI) * Mathf.Rad2Deg;
            orientation += (angle) * Mathf.Rad2Deg - 180; 
            Debug.Log("orientation: "+orientation);

            Debug.Log("radius "+radius);

            // Calculate the x and z coordinates
            float x = radius * Mathf.Sin(angle);
            float z = radius * Mathf.Cos(angle);
            Debug.Log("x: "+x);
            Debug.Log("z: "+z);

            // Create the new point
            Vector3 point = new Vector3(center.x + x, orientation, center.z + z);
            return point;
        }
    }
}
