using System;
using _Scripts.Manager;
using _Scripts.StateMachine;
using UnityEngine;

namespace _Scripts.State
{
    public class CalendarStateMachine : StateMachine<CalendarStateMachine, AppState>
    {
        [SerializeField] private Canvas _calendarCanvas;
        [SerializeField] private SwipeVerticalHorizontalMenu _calendarMenu;
        
        
        public void Awake()
        {
            _calendarMenu?.verticalPages[0]?.onSelectEvent.AddListener(() =>
            {
                _calendarCanvas.gameObject.SetActive(false);    
                ApplicationManager.Instance.SetState(AppState.Home);
            });
            AddToFunctionQueue(OnSelect, StateEvent.OnEnter);
            AddToFunctionQueue(OnDeselect, StateEvent.OnExit);
        }

        public void OnSelect(AppState exitState)
        {
            _calendarCanvas.gameObject.SetActive(true);
            _calendarMenu.Show();
        }
    
        public void OnDeselect(AppState enterState)
        {
            _calendarMenu.Hide();
        }
        
    }
}
