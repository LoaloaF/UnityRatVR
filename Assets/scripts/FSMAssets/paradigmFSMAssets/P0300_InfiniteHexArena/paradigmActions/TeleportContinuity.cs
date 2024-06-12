using FSM;
using System;
using System.Diagnostics;
using UnityEngine;
using RatVR.Scene;


namespace Experiment.ExperimentFSM

{
    [CreateAssetMenu(menuName = "FSM/Actions/TeleportContinuity")]
    public class TeleportContinuity : FSMAction
    {
        private Vector2[] hexagonVertices;
        public override void Execute(BaseStateMachine stateMachine)
        {
            hexagonVertices = InitializeVertices(stateMachine);

            Vector2 position = new Vector2(stateMachine._playerMovement.transform.position.x, stateMachine._playerMovement.transform.position.z);

            

            // Check each edge of the hexagon
            for (int i = 0; i < hexagonVertices.Length; i++)
            {
                Vector2 start = hexagonVertices[i];
                Vector2 end = hexagonVertices[(i + 1) % hexagonVertices.Length];
                Vector2 edge = end - start;
                Vector2 playerToStart = position - start;

                // Calculate the projection of the player's position onto the edge normal
                float projection = Vector2.Dot(playerToStart, new Vector2(-edge.y, edge.x).normalized);

                if (projection < 0)
                {

                    Vector2 directionToMove = new Vector2(-edge.y, edge.x).normalized * 34f * stateMachine._sceneController.scene.BaseLength;
                    UnityEngine.Debug.Log("directionToMove: " + directionToMove);
                    stateMachine._playerMovement.TeleportRat(stateMachine._playerMovement.transform.position.x + directionToMove.x,
                                                             stateMachine._playerMovement.transform.position.z + directionToMove.y,
                                                             stateMachine._playerMovement.transform.localRotation.eulerAngles.y);

                    GameObject[] landmarks = GameObject.FindGameObjectsWithTag("Landmark");

                    foreach (GameObject landmark in landmarks)
                    {
                        landmark.transform.position = new Vector3(landmark.transform.position.x + directionToMove.x,
                                                                  landmark.transform.position.y,
                                                                  landmark.transform.position.z + directionToMove.y);
                    }

                    break;
                }
            }
        }

        private Vector2[] InitializeVertices(BaseStateMachine stateMachine)
        {
            Vector2[] vertices = new Vector2[6];
            vertices[0] = new Vector2(-20f, 0f) * stateMachine._sceneController.scene.BaseLength;
            vertices[1] = new Vector2(-10f, -17.32f) * stateMachine._sceneController.scene.BaseLength;
            vertices[2] = new Vector2(10f, -17.32f) * stateMachine._sceneController.scene.BaseLength;
            vertices[3] = new Vector2(20f, 0f) * stateMachine._sceneController.scene.BaseLength;
            vertices[4] = new Vector2(10f, 17.32f) * stateMachine._sceneController.scene.BaseLength;
            vertices[5] = new Vector2(-10f, 17.32f) * stateMachine._sceneController.scene.BaseLength;

            return vertices;
        }

    }
}
