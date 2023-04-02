using System.Collections;
using System.Collections.Generic;
using _Scripts.StateMachine;
using UnityEngine;

public enum AppState
{
    Home,
    Navigate,
    Info,
    Search,
    Calendar
}

public class ApplicationManager : PersistentSingleton<ApplicationManager>
{
    private StateMachine<AppState> _stateMachine = new();
    
    void Start()
    {
        
    }

    
}
