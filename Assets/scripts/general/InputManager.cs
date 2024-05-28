using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FSM;

public class InputManager : MonoBehaviour
{
    // Start and stop session UI
    public Button startSessionButton;
    public Button stopSessionButton;
    
    // Success UI
    public TMP_InputField SuccessMagnitudeInput;
    public TMP_InputField SuccessDelayInput;
    
    // Punishment UI
    public TMP_InputField PunishmentMagnitudeInput;
    
    // Teleport UI
    public TMP_InputField TeleportXInput;
    public TMP_InputField TeleportZInput;
    public TMP_InputField TeleportAngleInput;


    public MeshRenderer frameIndicationBlinker;
    public MeshRenderer validationSphereRenderer; // MeshRenderer object that you can assign in the UI
    public GameObject Player;
    private PlayerMovement _playerMovement;
    private SessionManager _sessionManager;
    
    private PortentaInputInterface _portentaInputInterface;
    
    public GameObject UIObject;
    public bool showUI = true;
    public string paradigm_name = "P0100_Test";

    
    private CyclicPackagesSHMInterface unityInputSHMInterface;
    private FlagSHMInterface termflagSHMInterface;
    
    // Start is called before the first frame update
    void Start()
    {
        // Pause the game at the start
        Time.timeScale = 0;

        _playerMovement = Player.GetComponent<PlayerMovement>();
        _sessionManager = GetComponent<SessionManager>();
        _portentaInputInterface = GetComponent<PortentaInputInterface>();
        unityInputSHMInterface = new CyclicPackagesSHMInterface("unityinput_shmstruct.json");
        termflagSHMInterface = new FlagSHMInterface("termflag_shmstruct.json");

        Debug.Log("Clearing Input SHM");
        while (unityInputSHMInterface.Popitem() != null);

    }
    // Update is called once per frame
    void Update()
    {
        if (checkTermFlag()) {
            Application.Quit();
        }

        if (showUI && UIObject.activeSelf==false) {
            UIObject.SetActive(true);
        } else if (!showUI && UIObject.activeSelf==true) {
            UIObject.SetActive(false);
        }
        
        if (!showUI) processSHMInput();
            processSHMInput();
    }
    private bool checkTermFlag()
    {
        // Debug.Log("Checking termflag");
        // Debug.Log(termflagSHMInterface.IsSet());
        return termflagSHMInterface.IsSet();
    }

    private void processSHMInput()
    {
        string shmUnityInput;
        string[] splitInput;
        string command;
        float value1 = 0;
        float value2 = 0;
        float value3 = 0;

        shmUnityInput = unityInputSHMInterface.Popitem();

        if (shmUnityInput == null) return;
        Debug.Log(shmUnityInput);
        if (shmUnityInput == "Start") {
            StartGame();
        } else if (shmUnityInput == "Stop") {
            StopGame();
        } else if (shmUnityInput == "Failure") {
            sendFailure();
        } else if (shmUnityInput == "Airvalve") {
            sendSwitchAirvalve();
        } else if (shmUnityInput.StartsWith("Paradigm")) {
            Debug.Log(shmUnityInput.Split(','));
            paradigm_name = shmUnityInput.Split(',')[1];
        } else if (shmUnityInput.StartsWith("Punishment") 
                    || shmUnityInput.StartsWith("Success") 
                    || shmUnityInput.StartsWith("TrialEndTeleportDistanceDelta") 
                    || shmUnityInput.StartsWith("TrialEndTeleportAngleDelta") 
                    || shmUnityInput.StartsWith("Teleport")){
            try {
                splitInput = shmUnityInput.Split(',');
                command = splitInput[0];
                value1 = float.Parse(splitInput[1]);
                // more than a single value within message
                if (command == "Success" || command == "Teleport") {
                    value2 = float.Parse(splitInput[2]);
                    if (command == "Teleport") {
                        value3 = float.Parse(splitInput[3]);
                    }
                }
            } catch (FormatException) {
                Debug.LogError("Invalid values in message: " + shmUnityInput);
                return;
            }

            if (command == "Punishment") {
                _portentaInputInterface.sendPunishment((int)value1);
            } else if (command == "Success") {
                _portentaInputInterface.sendSuccess((int)value1, (int)value2);
            } else if (command == "Teleport") {
                _playerMovement.TeleportRat(value1, value2, value3);
            } else if (command == "TrialEndTeleportDistanceDelta") {
                _sessionManager.updateTrialEndTeleportCenterDist(value1);
            } else if (command == "TrialEndTeleportAngleDelta") {
                _sessionManager.updateTrialEndTeleportCenterAngle(value1);
            }
        } else {
            Debug.LogError("Invalid command: " + shmUnityInput);
        }
    }

    // Function to start the game
    public void StartGame()
    {
        // // Unpause the game
        // Time.timeScale = 1;

        // Disable the start button
        _sessionManager.abortTrialFlag = false;
        startSessionButton.interactable = false;
        stopSessionButton.interactable = true;

        GetComponent<BaseStateMachine>().initializeBaseStateMachine(paradigm_name);
        Debug.Log("Session started with paradigm_name: " + paradigm_name);

    }
    public void StopGame()
    {
        // // Unpause the game
        // Time.timeScale = 0;

        // Disable the start button
        _sessionManager.abortTrialFlag = true;
        startSessionButton.interactable = true;
        stopSessionButton.interactable = false;
        Debug.Log("Session stopped");
    }
    
    public void sendSwitchAirvalve()
    {
        _portentaInputInterface.sendSwitchAirvalve();
    }
    
    public void sendSuccess()
    {
        int rewardDelay;
        int rewardMagnitude;

        try
        {
            rewardDelay = int.Parse(SuccessDelayInput.text);
            rewardMagnitude = int.Parse(SuccessMagnitudeInput.text);
        }
        catch (FormatException)
        {
            Debug.LogError("Input values must be integers. Defaulting to 100,100");
            rewardDelay = 100;
            rewardMagnitude = 100;
        }
        _portentaInputInterface.sendSuccess(rewardDelay, rewardMagnitude);
    }

    public void sendFailure() 
    {
        _portentaInputInterface.sendFailure();
    }
    
    public void sendPunishment() 
    {
        int punishmentLength;
        try
        {
            punishmentLength = int.Parse(PunishmentMagnitudeInput.text);
        }
        catch (FormatException)
        {
            Debug.LogError("Input value must be an integer. Defaulting to 100");
            punishmentLength = 100;
        }
        _portentaInputInterface.sendPunishment(punishmentLength);
    }
    
    public void Teleport()
    {
        float X;
        float Z;
        float angle;
        try {
            X = float.Parse(TeleportXInput.text);
            Z = float.Parse(TeleportZInput.text);
            angle = float.Parse(TeleportAngleInput.text);
        } catch (FormatException) {
            Debug.LogError("Input values must be floats. Defaulting to 0,0,0");
            X = 0;
            Z = 0;
            angle = 0;
        }
        _playerMovement.TeleportRat(X, Z, angle);
    }

    
}