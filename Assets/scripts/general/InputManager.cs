using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputManager : MonoBehaviour
{
    // Start and stop session UI
    public Button startSessionButton;
    public Button stopSessionButton;
    
    // Success UI
    public Button SuccessButton;
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
    public GameObject player;
    private PlayerMovement _playerMovement;
    
    public GameObject ExperimentCore;
    private PortentaInputInterface _portentaInputInterface;
    
    public bool sessionRunning = false;

    public GameObject UIObject;
    public bool showUI = true;

    
    private CyclicPackagesSHMInterface unityInputSHMInterface;
    

    // Start is called before the first frame update
    void Start()
    {
        // Pause the game at the start
        Time.timeScale = 0;

        _playerMovement = player.GetComponent<PlayerMovement>();
        unityInputSHMInterface = new CyclicPackagesSHMInterface("../tmp_shm_structure_JSONs/unityinput_shmstruct.json");
        _portentaInputInterface = ExperimentCore.GetComponent<PortentaInputInterface>();
    }
    // Update is called once per frame
    void Update()
    {
        if (showUI && UIObject.activeSelf==false) {
            UIObject.SetActive(true);
        } else if (!showUI && UIObject.activeSelf==true) {
            UIObject.SetActive(false);
        }
        
        // if the Unity UI isn't used, take input from the shared memory
        if (!showUI) processSHMInput();
        if (sessionRunning) switchBlinkerColor();
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
        } else if (shmUnityInput.StartsWith("Punishment") 
                    || shmUnityInput.StartsWith("Success") 
                    || shmUnityInput.StartsWith("Teleport")){
            try {
                splitInput = shmUnityInput.Split(',');
                command = splitInput[0];
                value1 = float.Parse(splitInput[1]);
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
            }
        } else {
            Debug.LogError("Invalid command: " + shmUnityInput);
        }
    }

    private void switchBlinkerColor()
    {
        if ( Time.frameCount%2 == 1) {
                frameIndicationBlinker.material.color = Color.white;
        } else {
            frameIndicationBlinker.material.color = Color.black;
        }
    }

    // Function to start the game
    public void StartGame()
    {
        
        // Unpause the game
        Time.timeScale = 1;

        // Disable the start button
        startSessionButton.interactable = false;
        stopSessionButton.interactable = true;

        validationSphereRenderer.enabled = false;
        sessionRunning = true;
        Debug.Log("Session started");

    }
    public void StopGame()
    {
        // Unpause the game
        Time.timeScale = 0;

        // Disable the start button
        startSessionButton.interactable = true;
        stopSessionButton.interactable = false;

        validationSphereRenderer.enabled = true;
        sessionRunning = false;
        _playerMovement.TeleportRat(0,0,0);
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