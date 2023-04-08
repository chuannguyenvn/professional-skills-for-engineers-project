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
        [SerializeField] public TStateEnum _myStateEnum;

        protected List<Func<object[],IEnumerator>> onEnterEvents = new();
        protected List<Func<object[],IEnumerator>> onExitEvents = new();
    
        
        public enum StateEvent
        {
            OnEnter,
            OnExit
        }

        public IEnumerator OnExitState(object [] parameters = null)
        {
            foreach (var exitEnumerator in onExitEvents)
            {
                yield return StartCoroutine(exitEnumerator.Invoke(parameters));
            }
        }
        
        public IEnumerator OnEnterState(object [] parameters = null)
        {
            foreach (var enterEnumerator in onEnterEvents)
            {
                yield return StartCoroutine(enterEnumerator.Invoke(parameters));
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
                    onEnterEvents.Add((_) => ConvertToIEnumerator(action));
                    break;
                case StateEvent.OnExit:
                    onExitEvents.Add((_) => ConvertToIEnumerator(action));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateEvent), stateEvent, null);
            }
        
        }
    
        /// <summary>
        /// Add a IEnumerator to the queue, this is the only way for multiple parameter to be pass in 
        /// </summary>
        public void AddToFunctionQueue(Func<object[], IEnumerator> coroutine, StateEvent stateEvent)
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
                    onEnterEvents.Add((_) => ConvertToIEnumerator(tween));
                    break;
                case StateEvent.OnExit:
                    onExitEvents.Add((_) => ConvertToIEnumerator(tween));
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