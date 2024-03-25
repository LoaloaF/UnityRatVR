using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public MeshRenderer validationSphereRenderer; // MeshRenderer object that you can assign in the UI
    public Button startSessionButton; // Start button that you can assign in the UI
    public Button stopSessionButton; // Start button that you can assign in the UI
    public MeshRenderer frameIndicationBlinker;
    
    public GameObject player;
    private PlayerMovement _playerMovement;
    
    public GameObject ExperimentCore;
    private PortentaInputInterface _portentaInputInterface;
    
    public bool sessionRunning = false;



    

    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = player.GetComponent<PlayerMovement>();
        _portentaInputInterface = ExperimentCore.GetComponent<PortentaInputInterface>();
        
        // Pause the game at the start
        Time.timeScale = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (sessionRunning) switchBlinderColor();
    }

    private void switchBlinderColor()
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

    }
    
    public void sendSwitchAirvalve()
    {
        _portentaInputInterface.sendSwitchAirvalve();
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

    }
}

//ok i set the meshRenderer in the GUI. Please now add two textures 