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
    [SerializeField] private TMP_InputField searchBar;
    [SerializeField] private Button _navigationButton;
    [SerializeField] private Button _calendarPageButton;
    
    private void Awake()
    {
        AddToFunctionQueue(OnShow, StateEvent.OnEnter);
    }

    void OnShow()
    {
        _homeMenuCanvas.alpha = 1;
        _homeMenuCanvas.interactable = true;
    }

    private void OnDisable()
    {
        _homeMenuCanvas.alpha = 0;
        _homeMenuCanvas.interactable = false;
    }
}
