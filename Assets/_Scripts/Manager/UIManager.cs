using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : Singleton<UIManager>
{
    public enum UIStateType
    {
        Navigate,
        Home, 
        Search,
        CalendarView,
        BuildingInfo
    }

    [Serializable]
    private class UIStateInfo
    {
        public UIStateType myState;
        public List<UIStateType> outStates;
        public UnityEvent onEnter;
        public UnityEvent onExit;
    }

    [SerializeField] private List<UIStateInfo> _uiStates;
    private UIStateInfo currentState;
    
    public void ChangeState(UIStateType uiStateChangeTo)
    {
        var nextStateInfo = GetAccess(uiStateChangeTo);
        if (nextStateInfo == null) return;

    }

    private UIStateInfo GetAccess(UIStateType uiStateChangeTo)
    {
        var currentUIStateInfo = _uiStates.Find(state => state.myState == currentState.myState);
        return currentUIStateInfo.outStates.Contains(uiStateChangeTo)? _uiStates.Find(state => state.myState == uiStateChangeTo) : null;
    }

}
