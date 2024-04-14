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
        [SerializeField] private BaseState _initialState;
        private Dictionary<Type, Component> _cachedComponents;
        private InputManager _inputManager;
        public SceneGeometryData scene;
        public int generalCurrentStateID;
        // public string mapath;

        public void initializeBaseStateMachine(string paradigm_name) {
            string excelFullFileName = $"./Paradigms/{paradigm_name}.xlsx";
            GetComponent<SceneController>().LoadExcelScene(excelFullFileName);
            


            // scene = GetComponent<SceneController>().scene;
            
            // CurrentState = initialStates.TryGetValue(paradigm_name);
            // Debug.Log("Initilized :");
            // Debug.Log(CurrentState);
            // Debug.Log(CurrentState.stateID);
        }

        
        private void Awake()
        {
            _cachedComponents = new Dictionary<Type, Component>();
        }

        private void Start()
        {
            _inputManager = GetComponent<InputManager>();
            scene = GetComponent<SceneController>().scene;
            CurrentState = _initialState;
            generalCurrentStateID = _initialState.stateID;
        }

        public BaseState CurrentState { get; set; }

        private void Update()
        {
            if (_inputManager.sessionRunning)
            {
                // Exectures all actions attached to the current state
                CurrentState.Execute(this);
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
        

    }
    
}