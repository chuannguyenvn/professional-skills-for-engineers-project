using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace _Scripts.StateMachine
{
    public abstract class StateMachine<TStateEnum> : MonoBehaviour where TStateEnum : Enum
    {
        [Header("State Machine ")]
        [SerializeField] public TStateEnum myStateEnum;

        protected List<IEnumerator> onEnterEvents = new();
        protected List<IEnumerator> onExitEvents = new();
    
        
        public enum StateEvent
        {
            OnEnter,
            OnExit
        }

        public IEnumerator OnExitState()
        {
            foreach (var enterEvent in onExitEvents)
            {
                yield return StartCoroutine(enterEvent);
            }
        }

        public IEnumerator OnEnterState()
        {
            foreach (var exitEvent in onEnterEvents)
            {
                yield return StartCoroutine(exitEvent);
            }
        }

    }
    
    public class StateMachine<TMonoBehavior, TStateEnum> : StateMachine<TStateEnum>
        where TMonoBehavior : StateMachine<TStateEnum>
        where TStateEnum : Enum 
    {
        public void AddToFunctionQueue(Action action, StateEvent stateEvent)
        {
            switch (stateEvent)
            {
                case StateEvent.OnEnter:
                    onEnterEvents.Add(ConvertToIEnumerator(action));
                    break;
                case StateEvent.OnExit:
                    onExitEvents.Add(ConvertToIEnumerator(action));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateEvent), stateEvent, null);
            }
        
        }

        public void AddToFunctionQueue(IEnumerator coroutine, StateEvent stateEvent)
        {
            switch (stateEvent)
            {
                case StateEvent.OnEnter:
                    onEnterEvents.Add(coroutine);
                    break;
                case StateEvent.OnExit:
                    onExitEvents.Add(coroutine);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateEvent), stateEvent, null);
            }
        }

        public void AddToFunctionQueue(Tween tween, StateEvent stateEvent)
        {
            switch (stateEvent)
            {
                case StateEvent.OnEnter:
                    onEnterEvents.Add(ConvertToIEnumerator(tween));
                    break;
                case StateEvent.OnExit:
                    onExitEvents.Add(ConvertToIEnumerator(tween));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateEvent), stateEvent, null);
            }
        }


        private IEnumerator ConvertToIEnumerator(Action action)
        {
            action.Invoke();
            yield return null;
        }
        private IEnumerator ConvertToIEnumerator(Tween tween)
        {   
            yield return tween.WaitForCompletion(); //Intentionally make them to Coroutine
        }

    }
}