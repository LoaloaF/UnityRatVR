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

    private float framePositionX;
    private float framePositionZ;
    private float frameAngle;
    private int frameCount;
    private int frameBallVelFirstPackID;
    private int frameBallVelLastPackID;
    private string framePackage;
    private int frameState;
    private float frameTime;

    private BaseStateMachine _stateMachine;
    private PlayerMovement _playerMovement;

    private CyclicPackagesSHMInterface unityOutputSHMInterface;
    private CyclicPackagesSHMInterface ReadUnityOutputSHMInterface;



    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = GetComponent<BaseStateMachine>();
        _playerMovement = player.GetComponent<PlayerMovement>();
        unityOutputSHMInterface = new CyclicPackagesSHMInterface("../tmp_shm_structure_JSONs/unityoutput_shmstruct.json");

        // ReadUnityOutputSHMInterface = new CyclicPackagesSHMInterface("../tmp_shm_structure_JSONs/unityoutput_shmstruct.json");
        
    }

    // Update is called once per frame
    void Update()
    {
        // var readOut = ReadUnityOutputSHMInterface.Popitem();
        // Debug.Log($"TestRead: {readOut}");
        frameTime = Time.realtimeSinceStartup;
        frameCount = Time.frameCount;
        framePositionX = player.transform.position.x;
        framePositionZ = player.transform.position.z;
        frameAngle = player.transform.rotation.eulerAngles.y;
        frameBallVelFirstPackID = _playerMovement.firstPackID;
        frameBallVelLastPackID = _playerMovement.lastPackID;
        frameState = _stateMachine.CurrentState.stateID;
        
        framePackage = $"N:U,ID:{frameCount},PCT:{frameTime},X:{framePositionX},"+
                       $"Z:{framePositionZ},A:{frameAngle},S:{frameState},"+
                       $"BFP:{frameBallVelFirstPackID},BLP:{frameBallVelLastPackID}";
        Debug.Log($"Calling Push with {framePackage}");
        unityOutputSHMInterface.Push("<{"+framePackage+"}>\r\n");
    }
}

