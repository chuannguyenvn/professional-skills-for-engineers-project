using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.StateMachine;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Manager
{
    public enum AppState
    {
        Home,
        Navigate,
        Info,
        Search,
        Calendar
    }

    public class ApplicationManager : StateMachineManager<ApplicationManager, AppState>
    {
        [SerializeField] private StateMachine<MonoBehaviour,AppState> initStateMachine;
        void Start()
        {
            currentStateMachine = initStateMachine;
            var states = FindObjectsOfType<StateMachine<MonoBehaviour,AppState>>();
            foreach (var state in states)
            {
                AddState(state.myStateEnum, state);
            }
        }

    
    }
}