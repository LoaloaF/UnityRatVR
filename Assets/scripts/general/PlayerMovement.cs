using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class PlayerMovement : MonoBehaviour
{
    private bool movementEnabled = false;
    public CharacterController controller;
    public int firstPackID;
    public int lastPackID;
    public float wallZoneStopDistanceRatio = 0.2f;
    private int[] XYZvelInput = new int[3];
    public Vector3 gain = new Vector3(1f, 1f, 1f);
    private float rotY = 0f;
    private string log;

    // [Tooltip("Sensitivity scaler for the ball readout, higher the value, less sensitive it is")]
    //Normalize sensor inputs by multiplying: {'ballY': 0.01587624672951014, 'ballX': 0.01605927252520382, 'ballZ': 0.017359466370467733}
    private float ballForwardNormToCentimeter = 0.001542F;
    private float ballSidewaysNormToCentimeter = 0.001478F;
    private float ballRotatationNormToCentimeter = 0.003806F;

    [Tooltip("Weather to try to read the BallSensor, use WASD otherwise")]
    [SerializeField] bool enableBallInput = false;
    CyclicPackagesSHMInterface ballVelSHMInterface;

    void Start()
    {
        ballVelSHMInterface = new CyclicPackagesSHMInterface("ballvelocity_shmstruct.json");
    }

    // Update is called once per frame
    void Update() {
        if (movementEnabled) {
            XYZvelInput = getInput();
            // Debug.Log(string.Join(", ", XYZvelInput));
            MoveRat();
            RotateRat();
        }
        // Debug.Log(controller.velocity);
    }

    public int[] GetBallXYZVelocities() 
    {
        int i = 0;
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        int[] frameBallVel = new int[3];
        int[] ballVel = new int[3];
        int ballVelPackID;
        while (true)
        {
            (ballVel, ballVelPackID) = ballVelSHMInterface.fastPopBallVelocity();

            if (ballVelPackID == -1) break;
            if (i == 0) firstPackID = ballVelPackID;
            lastPackID = ballVelPackID;

            frameBallVel[0] += ballVel[0];
            frameBallVel[1] += ballVel[1];
            frameBallVel[2] += ballVel[2];
            i++;
        }
        stopwatch.Stop();
        // Debug.Log($"Got {i} BVs ({firstPackID}-{lastPackID}) in {stopwatch.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000)} μs: {frameBallVel}");
        return frameBallVel;
    }
    
    public void EnableMovement() {
        movementEnabled = true;
    }
    
    public void DisableMovement() {
        movementEnabled = false;
    }

    // read input
    private int[] getInput() {
        if (enableBallInput) {
            if (!(ballVelSHMInterface == null)) {
                XYZvelInput = GetBallXYZVelocities();
            } else {
                Debug.Log("SHM not linked. Can't read ball velocity");
            }
        } else {
            XYZvelInput = getKeyboardInput();
        }
        return XYZvelInput;
    }

    private int[] getKeyboardInput() {
        // mimic X input dimension/ forward rotation
        if (Input.GetKey(KeyCode.A)) {
            XYZvelInput[0] = 50;
        } else if (Input.GetKey(KeyCode.D)) {
            XYZvelInput[0] = -50;
        } else {
            XYZvelInput[0] = 0;
        }
        
        // mimic Y input dimension/ sideway rotation
        if (Input.GetKey(KeyCode.W)) {
            XYZvelInput[1] = 50;
        } else if (Input.GetKey(KeyCode.S)) {
            XYZvelInput[1] = -50;
        } else {
            XYZvelInput[1] = 0;
        }

        // mimic Z input dimension/ stationary/self rotation
        if (Input.GetKey(KeyCode.Q)) {
            XYZvelInput[2] = 50;
        } else if (Input.GetKey(KeyCode.E)) {
            XYZvelInput[2] = -50;
        } else {
            XYZvelInput[2] = 0;
        }
        return XYZvelInput;
    }

    // add Y input of ball to current forward vector (blue) and the same for right 
    private void MoveRat() {
        Vector3 forwardVel = Vector3.Scale(transform.forward*XYZvelInput[0]*ballForwardNormToCentimeter, gain);
        // Debug.Log("Velo " + controller.velocity);
        // Debug.Log("Forward " +forwardVel);
        Vector3 rightVel = Vector3.Scale(-transform.right*XYZvelInput[2]*ballSidewaysNormToCentimeter,gain);
        // Debug.Log("Right " + rightVel);
        controller.Move((forwardVel+rightVel));
    }

    // add Z input of ball to current y rotation
    private void RotateRat() {
        rotY += -(XYZvelInput[1]*ballRotatationNormToCentimeter)*gain.z;
        transform.localRotation = Quaternion.Euler(0f, rotY, 0f);
    }
    
    public void TeleportRat(float X, float Z, float angle) {
        Debug.Log("Teleporting to X: " + X + " Z: " + Z + " Angle: " + angle);

        controller.enabled = false;
        transform.position = new Vector3(X, transform.position.y, Z);
        controller.enabled = true;
        rotY = angle;
    }

}