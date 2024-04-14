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



        
        private void Awake()
        {
            CurrentState = _initialState;
            _cachedComponents = new Dictionary<Type, Component>();

        }

        private void Start()
        {
            scene = GetComponent<SceneController>().scene;
            _inputManager = GetComponent<InputManager>();
            generalCurrentStateID = _initialState.stateID;

        }

        public BaseState CurrentState { get; set; }

        private void Update()
        {
            if (_inputManager.sessionRunning)
            {
                // Exectures all actions attached to the current state
                CurrentState.Execute(this);
                //Debug.Log(CurrentState);
                //var positions = transform.GetComponentInChildren<Transform>().localPosition;
                //Debug.Log(positions);
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