using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;
using System;
using FSM;

// for more accuracy:
// https://forum.unity.com/threads/update-for-frame-timing-manager.1191877/

public class UnityFrameLogger : MonoBehaviour
{
    public GameObject player;
    private InputManager _inputManager;

    private float framePositionX;
    private float framePositionZ;
    private float frameAngle;
    private int frameCount;
    private int frameBallVelFirstPackID;
    private int frameBallVelLastPackID;
    private string framePackage;
    private int frameState;
    private float frameTime;
    private int blinkerState;

    private BaseStateMachine _stateMachine;
    private PlayerMovement _playerMovement;

    private CyclicPackagesSHMInterface unityOutputSHMInterface;
    private CyclicPackagesSHMInterface ReadUnityOutputSHMInterface;

    public MeshRenderer frameIndicationBlinker;


    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = GetComponent<BaseStateMachine>();
        _playerMovement = player.GetComponent<PlayerMovement>();
        _inputManager = GetComponent<InputManager>();
        unityOutputSHMInterface = new CyclicPackagesSHMInterface("../tmp_shm_structure_JSONs/unityoutput_shmstruct.json");
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputManager.sessionRunning)
        {
            LogFrame();
        }
    }

    void LogFrame() {
        // var readOut = ReadUnityOutputSHMInterface.Popitem();
        // Debug.Log($"TestRead: {readOut}");
        frameTime = Time.realtimeSinceStartup;
        frameCount = Time.frameCount;
        framePositionX = player.transform.position.x;
        framePositionZ = player.transform.position.z;
        frameAngle = player.transform.rotation.eulerAngles.y;
        frameBallVelFirstPackID = _playerMovement.firstPackID;
        frameBallVelLastPackID = _playerMovement.lastPackID;
        frameState = _stateMachine.generalCurrentStateID;

        // set blinker to 0 if frameIndicationBlinker.Color == Color.black else set it to 1
        blinkerState = frameIndicationBlinker.material.color == Color.black ? 0 : 1;
        
        framePackage = $"N:U,ID:{frameCount},PCT:{frameTime},X:{framePositionX},"+
                    $"Z:{framePositionZ},A:{frameAngle},S:{frameState},FB:{blinkerState},"+
                    $"BFP:{frameBallVelFirstPackID},BLP:{frameBallVelLastPackID}";

        // Debug.Log($"Calling Push with {framePackage}");
        Debug.Log(frameState);
        unityOutputSHMInterface.Push("<{"+framePackage+"}>\r\n");
    }
}

