using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.StateMachine
{
    public class StateMachineManager<TMonoBehaviour,TStateEnum> : PersistentSingleton<TMonoBehaviour> where TStateEnum : Enum where TMonoBehaviour : MonoBehaviour
    {
        protected StateMachine<TStateEnum> currentStateMachine;
        private Dictionary<TStateEnum, StateMachine<TStateEnum>> _states = new ();

        private Queue<StateMachine<TStateEnum>> _changingStateQueue = new ();
        private bool _isChangingState;
        
        public void AddState(TStateEnum stateEnum, StateMachine<TStateEnum> stateMachine)
        {
            _states[stateEnum] = stateMachine;
        }

        public void RemoveState(TStateEnum stateEnum)
        {
            _states.Remove(stateEnum);
        }

        public void SetState(TStateEnum stateEnum)
        {
            if (_states.TryGetValue(stateEnum, out StateMachine<TStateEnum> nextState))
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
                yield return StartCoroutine(currentStateMachine.OnExitState());
                yield return StartCoroutine(nextState.OnEnterState());
                currentStateMachine = nextState;
            }

            _isChangingState = false;
        }
        
    }
}
