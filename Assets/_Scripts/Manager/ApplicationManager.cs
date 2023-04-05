using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Search_Bar;
using _Scripts.StateMachine;
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
        [SerializeField] private AppState initState;

        [SerializeField] private List<StateMachine<AppState>> appStateMachines;
        void Start()
        {
            currentStateMachine = appStateMachines.Find(state => state.myStateEnum == initState);
            appStateMachines = FindObjectsOfType<StateMachine<AppState>>().ToList();
            foreach (var stateMachine in appStateMachines)
            {
                AddState(stateMachine.myStateEnum, stateMachine);
            }
            
        }

    }
}