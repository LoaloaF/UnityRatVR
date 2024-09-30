using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cathei.BakingSheet;
using RatVR.ExcelData;
using RatVR.Scene;
using System.Collections;

namespace FSM
{
    public class BaseStateMachine : MonoBehaviour
    {
        /*
         * This class is the base class of the finite state machine. It needs to be applied to a gameobject in the scene.
         * Then one can create states, transitions and decision rules in the editor and build the FSM.
         */
        // [SerializeField] private BaseState _initialState;
        public GameObject Player;
        [HideInInspector] public PlayerMovement _playerMovement;

        public MeshRenderer frameBlinkerWhite;
        public MeshRenderer frameBlinkerBlack;
        public bool isWhiteActive;
        public MeshRenderer validationSphereRenderer; // MeshRenderer object that you can assign in the UI
        private Dictionary<Type, Component> _cachedComponents;
        private InputManager _inputManager;
        private FlagSHMInterface startflagSHMInterface;
        private bool startFlag = false;

        [HideInInspector] public SceneController _sceneController;
        [HideInInspector] public SessionManager _sessionManager;

        public int generalCurrentStateID;
        public StateDictionary paradigmDictionary;
        public BaseState CurrentState { get; set; }
        public int startFrameID;
        public int currentFrameID;

        public void initializeBaseStateMachine(string paradigm_name) {
            
            // find the excel file in the project folder
            // this adjusts the path when exec from build subfolder 
            string projectPath = Path.GetDirectoryName(Application.dataPath);
            if (Directory.Exists(Path.Combine(projectPath, "Assets")) == false)
                projectPath = Path.Combine(projectPath, "..");
            if (Directory.Exists(Path.Combine(projectPath, "Assets")) == false)
                projectPath = Path.Combine(projectPath, "..");

            string excelFullFileName = Path.Combine(projectPath, "Paradigms", $"{paradigm_name}.xlsx");

            // load the scene from the excel file
            _sceneController.LoadExcelScene(excelFullFileName);
            
            CurrentState = paradigmDictionary.TryGetValue(paradigm_name);
            Debug.Log($"Initial state: {CurrentState}");
            generalCurrentStateID = CurrentState.stateID;
            _sessionManager.sessionRunning = true;
            // StartCoroutine(DelayFrameLogging(5f));

            Time.timeScale = 1;
            _playerMovement.EnableMovement();

        }
        
        private void Awake()
        {
            _cachedComponents = new Dictionary<Type, Component>();
            _sceneController = GetComponent<SceneController>();
            _sessionManager = GetComponent<SessionManager>();
            _playerMovement = Player.GetComponent<PlayerMovement>();
        }

        private void Start()
        {
            _inputManager = GetComponent<InputManager>();
            startflagSHMInterface = new FlagSHMInterface("paradigmflag_shmstruct.json");
            startFlag = false;
            frameBlinkerWhite.material.color = Color.white;
            frameBlinkerBlack.material.color = Color.black;
            isWhiteActive = false;
            frameBlinkerBlack.gameObject.SetActive(true);
            frameBlinkerWhite.gameObject.SetActive(false);
            startFrameID = -1;
            currentFrameID = -1;
        }


        private void Update()
        {
            if (checkStartFlag() && !startFlag)
            {
                System.Threading.Thread.Sleep(1200);
                StartGame();
            }
            else if (!checkStartFlag() && startFlag)
            {
                StopGame();
            }

            if (_sessionManager.sessionRunning)
            {
                // Exectures all actions attached to the current state
                CurrentState.Execute(this);
                generalCurrentStateID = CurrentState.stateID;
                if (currentFrameID - startFrameID > 120)
                    switchBlinkerColor();
            }
        }
        
        public void StartGame()
        {
            startFlag = true;
            _sessionManager.abortTrialFlag = false;
            startFrameID = Time.frameCount;
            _inputManager.startSessionButton.interactable = false;
            _inputManager.stopSessionButton.interactable = true;
            initializeBaseStateMachine(_inputManager.paradigm_name);
            Debug.Log("Session started with paradigm_name: " + _inputManager.paradigm_name);

        }
        public void StopGame()
        {
            startFlag = false;
            _sessionManager.abortTrialFlag = true;
            _inputManager.startSessionButton.interactable = true;
            _inputManager.stopSessionButton.interactable = false;
            _sessionManager.sessionRunning = false;
            Debug.Log("Session stopped");
        }



        private bool checkStartFlag()
        {
            return startflagSHMInterface.IsSet();
        }


        public new T GetComponent<T>() where T : Component
        {
            if(_cachedComponents.ContainsKey(typeof(T)))
                return _cachedComponents[typeof(T)] as T;

            var component = base.GetComponent<T>();
            if(component != null)
            {
                _cachedComponents.Add(typeof(T), component);
            }
            return component;
        }

        private void switchBlinkerColor()
        {
            if (isWhiteActive)
            {
                frameBlinkerBlack.gameObject.SetActive(true);
                frameBlinkerWhite.gameObject.SetActive(false);
                isWhiteActive = false;
            }
            else
            {
                frameBlinkerBlack.gameObject.SetActive(false);
                frameBlinkerWhite.gameObject.SetActive(true);
                isWhiteActive = true;
            }
        }

    }

    [System.Serializable] public class StateDictionary
    {
        [SerializeField]
        private List<string> keys = new List<string>();

        [SerializeField]
        private List<State> values = new List<State>();

        public void Add(string key, State value)
        {
            keys.Add(key);
            values.Add(value);
        }

        public State? TryGetValue(string key)
        {
            State value;
            int index = keys.IndexOf(key);
            foreach (string k in keys)
            {
                // Debug.Log($"Key: '{k}'");
            }
            if (index >= 0)
            {
                value = values[index];
            }
            else
            {
                Debug.LogError($"State {key} not found");
                value = null;
            }
            return value;
        }
    }

}