using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.StateMachine
{
    public abstract class State : MonoBehaviour
    {
        public enum StateEvent
        {
            OnEnter,
            OnExit
        }
        protected List<Func<IEnumerator>> onEnterEvents = new();
        protected List<Func<IEnumerator>> onExitEvents = new();
        private Coroutine _currentQueueCoroutine;
    
        public IEnumerator OnExitState()
        {
            foreach (var enterEvent in onExitEvents)
            {
                yield return StartCoroutine(enterEvent.Invoke());
            }
        }

        public IEnumerator OnEnterState()
        {
            foreach (var exitEvent in onEnterEvents)
            {
                yield return StartCoroutine(exitEvent.Invoke());
            }
        }
    
        public void AddToFunctionQueue(System.Action action, StateEvent stateEvent)
        {
            switch (stateEvent)
            {
                case StateEvent.OnEnter:
                    onEnterEvents.Add(() =>
                    {
                        action.Invoke();
                        return null;
                    });
                    break;
                case StateEvent.OnExit:
                    onExitEvents.Add(() =>
                    {
                        action.Invoke();
                        return null;
                    });
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
                    onEnterEvents.Add(() => coroutine);
                    break;
                case StateEvent.OnExit:
                    onExitEvents.Add(() => coroutine);
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
                    onEnterEvents.Add(() => WaitForTween(tween));
                    break;
                case StateEvent.OnExit:
                    onExitEvents.Add(() => WaitForTween(tween));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateEvent), stateEvent, null);
            }
        }

        private IEnumerator WaitForTween(Tween tween)
        {   
            yield return tween.WaitForCompletion(); //Intentionally make them to Coroutine
        }

    }
}