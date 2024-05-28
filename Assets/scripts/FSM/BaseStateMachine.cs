using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cathei.BakingSheet;
using RatVR.ExcelData;
using RatVR.Scene;

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

        public MeshRenderer frameIndicationBlinker;
        public MeshRenderer validationSphereRenderer; // MeshRenderer object that you can assign in the UI
        private Dictionary<Type, Component> _cachedComponents;
        private InputManager _inputManager;

        [HideInInspector] public SceneController _sceneController;
        [HideInInspector] public SessionManager _sessionManager;
        public int generalCurrentStateID;
        public StateDictionary stateDictionary;
        public BaseState CurrentState { get; set; }

        public void initializeBaseStateMachine(string paradigm_name) {
            
            // find the excel file in the project folder
            // this adjusts the path when exec from build subfolder 
            string projectPath = Path.GetDirectoryName(Application.dataPath);
            if (Directory.Exists(Path.Combine(projectPath, "Assets")) == false) {
                projectPath = Path.Combine(projectPath, "..");
            }
            string excelFullFileName = Path.Combine(projectPath, "Paradigms", $"{paradigm_name}.xlsx");

            // load the scene from the excel file
            _sceneController.LoadExcelScene(excelFullFileName);
            
            CurrentState = stateDictionary.TryGetValue(paradigm_name);
            Debug.Log($"Initial state: {CurrentState}");
            generalCurrentStateID = CurrentState.stateID;
            _sessionManager.sessionRunning = true;

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
        }


        private void Update()
        {
            if (_sessionManager.sessionRunning)
            {
                // Exectures all actions attached to the current state
                CurrentState.Execute(this);
                generalCurrentStateID = CurrentState.stateID;
                switchBlinkerColor();
            }
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
            if ( Time.frameCount%2 == 1) {
                    frameIndicationBlinker.material.color = Color.white;
            } else {
                frameIndicationBlinker.material.color = Color.black;
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