using FSM;
using JetBrains.Annotations;
using RatVR.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.IO.Ports;
using System.Text;

namespace Experiment.ExperimentFSM
{
    [CreateAssetMenu(menuName = "FSM/Actions/P0800/P0800_TrialInitLinearTrack")]

    public class P0800_TrialInitLinearTrack : FSMAction
    {
        public GameObject trackWall;
        public P0800_FadeInCue fadeInCue1;
        public P0800_FadeInCue fadeInCue2;
        public CyclicPackagesSHMInterface portentaOutputSHMInterface;
        public Dictionary<string, MeshRenderer> pillarCylinderMeshes = new Dictionary<string, MeshRenderer>();


        public SerialPort serialPort;
        public bool pumpOpened = false;


        public override void Execute(BaseStateMachine stateMachine)
        {
            // initialize the syringe pump
            // Serial port settings
            string portName = "COM3";  // Replace with your serial port name (e.g., "COM3" for Windows)
            int baudRate = 115200;
            float velocity = 70.0f; // ml/min maximum velocity
            int withdrawalAmount = 100; // uL

            try
            {
                // Initialize and open the serial port
                serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
                serialPort.Encoding = Encoding.UTF8;
                serialPort.ReadTimeout = 1000; // 1 second timeout for read/write operations
                serialPort.Open();
                if (serialPort.IsOpen)
                {
                    Debug.Log("Serial port is open.");
                    pumpOpened = true;
                    // Create withdrawal command
                    string withdrawalCommand = string.Format("wit {0:0.0}ml/min {1}ul\n", velocity, withdrawalAmount);
                    // Send the withdrawal command
                    serialPort.Write(withdrawalCommand);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error opening serial port: " + e.Message);
                pumpOpened = false;
            }


            stateMachine._sceneController.floor.SetActive(true);
            stateMachine._sceneController.wallZone.SetActive(true);
            stateMachine.validationSphereRenderer.enabled = false;
            stateMachine._sceneController.ceiling.SetActive(true);
            stateMachine._sceneController.wallTop.SetActive(true);
            stateMachine._sceneController.wallBottom.SetActive(true);
            stateMachine._sceneController.wallLeft.SetActive(true);
            stateMachine._sceneController.wallRight.SetActive(true);
            stateMachine._playerMovement.EnableMovement();

            if (portentaOutputSHMInterface == null) portentaOutputSHMInterface = new CyclicPackagesSHMInterface("portentaoutput_shmstruct.json");

            stateMachine._playerMovement.ballSidewaysNormToCentimeter = 0f;
            stateMachine._playerMovement.ballRotatationNormToCentimeter = 0f;
            stateMachine._playerMovement.zOnlyMovePositive = true;

            GameObject[] pillars = GameObject.FindGameObjectsWithTag("Pillar");
            Debug.Log("Pillar count: " + pillars.Length);

            for (int pillarIdx = 1; pillarIdx < 17; pillarIdx++)
            {
                foreach (GameObject pillar in pillars)
                {   
                    if (pillar.name.StartsWith("Pillar" + pillarIdx.ToString() + "_"))
                    {
                        MeshRenderer[] meshRenderer = pillar.GetComponentsInChildren<MeshRenderer>();
                        foreach (MeshRenderer mesh in meshRenderer)
                        {
                            if (mesh.gameObject.name == "Cylinder")
                            {
                                pillarCylinderMeshes.Add(pillarIdx.ToString() , mesh);
                            }

                        }
                        break;
                    }
                    
                }
            }
 

            MeshRenderer pillar3Mesh = pillarCylinderMeshes["3"];
            MeshRenderer pillar4Mesh = pillarCylinderMeshes["4"];
            pillar3Mesh.material.mainTextureScale = new Vector2(3.37f, 3.37f);
            pillar4Mesh.material.mainTextureScale = new Vector2(3.37f, 3.37f);


            foreach (GameObject pillar in pillars)
            {
                if (pillar.name.StartsWith("Pillar0_"))
                {
                    pillar.SetActive(false);
                }

                else if (pillar.name.StartsWith("Pillar1_")|| pillar.name.StartsWith("Pillar2_"))
                {
                    MeshRenderer[] meshRenderer = pillar.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer mesh in meshRenderer)
                    {
                        if (pillar.name.StartsWith("Pillar1_"))
                        {
                            if (mesh.material.color.a == 1)
                                fadeInCue1.cueShouldFadeIn = true;
                            else
                                fadeInCue1.cueShouldFadeIn = false;  
                        }
                        else if (pillar.name.StartsWith("Pillar2_"))
                        {
                            if (mesh.material.color.a == 1)
                                fadeInCue2.cueShouldFadeIn = true;
                            else
                                fadeInCue2.cueShouldFadeIn = false;
                        }
                    }
                }
            }

            float xPos = 0;
            float ySize = 0;
            foreach (GameObject pillar in pillars)
            {
                if (pillar.name.StartsWith("Pillar0_"))
                {
                    xPos = pillar.transform.position.x;
                    ySize = pillar.transform.position.y;
                    break;
                }
            }

            float arenaSize = stateMachine._sceneController.scene.Size.x;
            Vector3 leftWallPosition = new Vector3(Math.Abs(xPos) * -1f, ySize+1, -1);
            Vector3 rightWallPosition = new Vector3(Math.Abs(xPos), ySize+1, -1);

            GameObject leftWall = Instantiate(trackWall, leftWallPosition, Quaternion.identity, stateMachine.transform);
            leftWall.name = "LeftWall";
            leftWall.transform.localScale = new Vector3(4, ySize*4, arenaSize);
            leftWall.GetComponentInChildren<MeshRenderer>().material = stateMachine._sceneController.materials["grey"];


            GameObject rightWall = Instantiate(trackWall, rightWallPosition, Quaternion.identity, stateMachine.transform);
            rightWall.name = "RightWall";
            rightWall.transform.localScale = new Vector3(4, ySize*4, arenaSize);
            rightWall.GetComponentInChildren<MeshRenderer>().material = stateMachine._sceneController.materials["grey"];

        }

    }
}
