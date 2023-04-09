using System.Collections;
using System.Collections.Generic;
using _Scripts.Manager;
using _Scripts.Map;
using _Scripts.StateMachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NavigateStateMachine : StateMachine<NavigateStateMachine, AppState>
{
    [SerializeField] private Canvas _navigateCanvas;
    [SerializeField] private CanvasGroup _navigateECanvasGroup;
    [SerializeField] private TMP_Text _buildingText;
    [SerializeField] private Button _backButton;
    [SerializeField] private float _transitionDuration;

    [SerializeField] private PlayerNavigation _playerNavigation;
    
    public void Awake()
    {
        _backButton?.onClick.AddListener(() => ApplicationManager.Instance.SetToLastState());
        AddToFunctionQueue(OnSelect, StateEvent.OnEnter);
        AddToFunctionQueue(OnDeselect, StateEvent.OnExit);
    }
    
    public void OnSelect(AppState exitState, object[] parameters)
    {
        Building building = parameters[0] as Building;

        if (building == null) return;

        _buildingText.text = building.buildingSo.buildingName;
        _playerNavigation.EnableNavigation(building);
        
        _navigateCanvas.gameObject.SetActive(true);
        _navigateECanvasGroup.interactable = true;
        
        _navigateECanvasGroup.alpha = 0;
        DOTween.To(() => _navigateECanvasGroup.alpha, x => _navigateECanvasGroup.alpha = x, 1, _transitionDuration);
    }
    
    public void OnDeselect(AppState enterState, object [] parameters)
    {
        _playerNavigation.DisableNavigation();
        
        _navigateECanvasGroup.interactable = false;
        _navigateECanvasGroup.alpha = 1;
        DOTween.To(() => _navigateECanvasGroup.alpha, x => _navigateECanvasGroup.alpha = x, 0, _transitionDuration).OnComplete(
            () => _navigateCanvas.gameObject.SetActive(false));   
    }
}
