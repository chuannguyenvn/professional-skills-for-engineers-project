using System.Collections;
using System.Collections.Generic;
using _Scripts.Manager;
using _Scripts.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NavigateStateMachine : StateMachine<NavigateStateMachine, AppState>
{
    [SerializeField] private Canvas _navigateCanvas;
    [SerializeField] private TMP_Text _buildingText;

    [SerializeField] private Button _backButton;
    
    public void Awake()
    {
        _backButton?.onClick.AddListener(() => ApplicationManager.Instance.SetState(AppState.Home));
        AddToFunctionQueue(OnSelect, StateEvent.OnEnter);
        AddToFunctionQueue(OnDeselect, StateEvent.OnExit);
    }

    public void OnSelect(AppState exitState)
    {
        
    }
    
    public void OnDeselect(AppState enterState)
    {
g        if(enterState != AppState.Calendar) _navigateCanvas.gameObject.SetActive(false);    
    }
}
