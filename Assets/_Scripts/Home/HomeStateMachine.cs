using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Manager;
using _Scripts.StateMachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HomeStateMachine : StateMachine<HomeStateMachine, AppState>
{
    [SerializeField] private CanvasGroup _homeMenuCanvas;
    [SerializeField] private TMP_InputField _searchBar;
    [SerializeField] private Button _navigationButton;
    [SerializeField] private Button _calendarPageButton;
    
    private void Awake()
    {
        InitEvent();
        AddToFunctionQueue(OnShow, StateEvent.OnEnter);
        AddToFunctionQueue(OnHide, StateEvent.OnExit);
    }

    private void InitEvent()
    {
        _navigationButton?.onClick.AddListener(()=> Debug.Log("Navigation in home"));
        _calendarPageButton?.onClick.AddListener(() => ApplicationManager.Instance.SetState(AppState.Calendar));
        _searchBar?.onSelect.AddListener((_) => ApplicationManager.Instance.SetState(AppState.Search));
    }
    
    void OnShow()
    {
        _homeMenuCanvas.alpha = 1;
        _homeMenuCanvas.interactable = true;
    }

    private void OnHide()
    {
        _homeMenuCanvas.alpha = 0;
        _homeMenuCanvas.interactable = false;
    }
}
