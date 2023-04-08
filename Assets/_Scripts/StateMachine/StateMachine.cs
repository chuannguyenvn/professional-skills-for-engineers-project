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

        public IEnumerator OnExitState(object [] parameters)
        {
            foreach (var exitEnumerator in onExitEvents)
            {
                yield return StartCoroutine(exitEnumerator.Invoke(parameters));
            }
        }
        
        public IEnumerator OnEnterState(object [] parameters)
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
        public void AddToFunctionQueue(Action action, StateEvent stateEvent, object[] parameters = null)
        {
            switch (stateEvent)
            {
                case StateEvent.OnEnter:
                    onEnterEvents.Add((_) => ConvertToIEnumerator(action,parameters));
                    break;
                case StateEvent.OnExit:
                    onExitEvents.Add((_) => ConvertToIEnumerator(action,parameters));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateEvent), stateEvent, null);
            }
        
        }

        public void AddToFunctionQueue(IEnumerator coroutine, StateEvent stateEvent, object[] parameters = null)
        {
            switch (stateEvent)
            {
                case StateEvent.OnEnter:
                    onEnterEvents.Add((_) => coroutine);
                    break;
                case StateEvent.OnExit:
                    onExitEvents.Add((_) => coroutine);
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


        private IEnumerator ConvertToIEnumerator(Action action, object[] parameters)
        {
            action.DynamicInvoke(parameters);
            yield return null;
        }
        private IEnumerator ConvertToIEnumerator(Tween tween)
        {   
            yield return tween.WaitForCompletion(); //Intentionally make them to Coroutine
        }

    }
}