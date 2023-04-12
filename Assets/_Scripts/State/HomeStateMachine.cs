using _Scripts.Manager;
using _Scripts.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.State
{
    public class HomeStateMachine : StateMachine<HomeStateMachine, AppState>
    {
        [SerializeField] private CanvasGroup _homeMenuCanvas;
        [SerializeField] private TMP_InputField _searchBar;
        [SerializeField] private Button _navigationButton;
        [SerializeField] private Button _calendarPageButton;
    
        private void Awake()
        {
            InitEvent();
            AddToFunctionQueue(OnSelect, StateEvent.OnEnter);
            AddToFunctionQueue(OnDeselect, StateEvent.OnExit);
        }

        private void InitEvent()
        {
            _navigationButton?.onClick.AddListener(()=> Debug.Log("Navigation in home"));
            _calendarPageButton?.onClick.AddListener(() => ApplicationManager.Instance.SetState(AppState.Calendar));
            _searchBar?.onSelect.AddListener((_) => ApplicationManager.Instance.SetState(AppState.Search));
        
        }
    
        void OnSelect(AppState exitState)
        {
            _homeMenuCanvas.alpha = 1;
            _homeMenuCanvas.interactable = true;
        }

        private void OnDeselect(AppState enterState)
        {
            if(enterState != AppState.Search) _homeMenuCanvas.alpha = 0;
            _homeMenuCanvas.interactable = false;
        }
    }
}
