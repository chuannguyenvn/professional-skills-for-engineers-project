using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.StateMachine
{
    public class StateMachine<TState> : MonoBehaviour where TState : Enum
    {
        private Dictionary<TState, State> _states = new ();
        private State _currentState;

        private Queue<State> _changingStateQueue = new ();
        private bool _isChangingState;
        
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
            if (_states.TryGetValue(stateEnum, out State nextState))
            {
                _changingStateQueue.Enqueue(nextState);
            }
            else
            {
                Debug.LogWarning($"State {stateEnum} not found in state machine.");
            }
        }

        private IEnumerator SwitchingState()
        {
            if (_isChangingState)
            {
                yield break;
            }

            _isChangingState = true;
            while (_changingStateQueue.Count>0)
            {
                var nextState = _changingStateQueue.Dequeue();
                yield return StartCoroutine(_currentState.OnExitState());
                yield return StartCoroutine(nextState.OnEnterState());
                _currentState = nextState;
            }

            _isChangingState = false;
        }
        
    }
}
