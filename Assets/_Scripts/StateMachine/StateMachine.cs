using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.StateMachine
{
    public class StateMachine<TState> where TState : Enum  
    {
        private Dictionary<TState, State> _states = new ();
        private State _currentState;

        public void AddState(TState stateEnum, State state)
        {
            _states[stateEnum] = state;
        }

        public void RemoveState(TState stateEnum)
        {
            _states.Remove(stateEnum);
        }

        public void SetState(TState stateEnum)
        {
            if (_currentState != null)
            {
                _currentState.OnExitState();
            }

            if (_states.TryGetValue(stateEnum, out State nextState))
            {
                _currentState = nextState;
                _currentState.OnEnterState();
            }
            else
            {
                Debug.LogWarning($"State {stateEnum} not found in state machine.");
            }
        }

        public void Update()
        {
            if (_currentState != null)
            {
            
            }
        }
    }
}
