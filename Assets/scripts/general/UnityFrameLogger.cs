using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;
using FSM;

// for more accuracy:
// https://forum.unity.com/threads/update-for-frame-timing-manager.1191877/

public class UnityFrameLogger : MonoBehaviour
{
    public GameObject Player;
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
    private bool isWhiteActive;

    private BaseStateMachine _stateMachine;
    private PlayerMovement _playerMovement;

    public CyclicPackagesSHMInterface unityOutputSHMInterface;
    private CyclicPackagesSHMInterface ReadUnityOutputSHMInterface;

    public MeshRenderer frameBlinkerWhite;
    public MeshRenderer frameBlinkerBlack;
    


    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = GetComponent<BaseStateMachine>();
        _playerMovement = Player.GetComponent<PlayerMovement>();
        unityOutputSHMInterface = new CyclicPackagesSHMInterface("unityoutput_shmstruct.json");

        frameBlinkerWhite.material.color = Color.black;
        // frameBlinkerBlack.material.color = Color.black;
        frameBlinkerWhite.gameObject.SetActive(true);
        frameBlinkerBlack.gameObject.SetActive(false);
        isWhiteActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_stateMachine._sessionManager.sessionRunning)
        {
            return;
        }
        else if (Time.frameCount - _stateMachine._sessionManager.startFrameID >= 120)
        {
            LogFrame();
        }
    }

    void LogFrame() {
        // var readOut = ReadUnityOutputSHMInterface.Popitem();

        // Debug.Log($"TestRead: {readOut}");
        // frameTime = Time.realtimeSinceStartup;
        DateTime currentDateTime = DateTime.UtcNow;
        long ticksSinceEpoch = currentDateTime.Ticks - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
        long unixTimestampMicroseconds = ticksSinceEpoch / 10;

        frameCount = Time.frameCount;
        framePositionX = Player.transform.position.x;
        framePositionZ = Player.transform.position.z;
        frameAngle = Player.transform.rotation.eulerAngles.y;
        frameBallVelFirstPackID = _playerMovement.firstPackID;
        frameBallVelLastPackID = _playerMovement.lastPackID;
        frameState = _stateMachine.generalCurrentStateID;


        // if ( Time.frameCount %2 == 1) {
        //     frameBlinkerWhite.gameObject.SetActive(false);
        //     frameBlinkerBlack.gameObject.SetActive(true);
        //     blinkerState = 0;
        // } else if (Time.frameCount %2== 0) {
        //     frameBlinkerWhite.gameObject.SetActive(true);
        //     frameBlinkerBlack.gameObject.SetActive(false);
        //     blinkerState = 1;
        // }

        if (isWhiteActive) {
            frameBlinkerWhite.material.color = Color.black;
            // frameBlinkerBlack.gameObject.SetActive(true);
            // blinkerState = 0;
            isWhiteActive = false;
        } else {
            frameBlinkerWhite.material.color = Color.white;
            // frameBlinkerBlack.gameObject.SetActive(false);
            // blinkerState = 1;
            isWhiteActive = true;
        }

        // set blinker to 0 if frameIndicationBlinker.Color == Color.black else set it to 1
        blinkerState = frameBlinkerWhite.material.color == Color.black ? 0 : 1;
        
        framePackage = $"N:U,ID:{frameCount},PCT:{unixTimestampMicroseconds},X:{framePositionX.ToString("F5")},"+
                    $"Z:{framePositionZ.ToString("F5")},A:{frameAngle.ToString("F5")},S:{frameState},FB:{blinkerState},"+
                    $"BFP:{frameBallVelFirstPackID},BLP:{frameBallVelLastPackID}";

        // Debug.Log($"Calling Push with {framePackage}");
        unityOutputSHMInterface.Push("<{"+framePackage+"}>\r\n");
    }
}