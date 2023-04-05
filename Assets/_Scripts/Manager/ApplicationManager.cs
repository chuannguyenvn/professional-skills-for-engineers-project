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
        [SerializeField] private AppState initState;

        [SerializeField] private List<StateMachine<MonoBehaviour, AppState>> appStateMachines;
        void Start()
        {
            currentStateMachine = appStateMachines.Find(state => state.myStateEnum == initState);
        }

    
    }
}