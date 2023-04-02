using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace _Scripts.UI
{
    public class SwipeVerticalMenu : MonoBehaviour, IPointerDownHandler, IDragHandler,
        IEndDragHandler
    {
        [Header("Animation Properties")] 
        [SerializeField] protected float movingDuration = 0.5f;
        [SerializeField] protected Ease ease = Ease.OutCubic;
        
        [Header("Vertical Properties")] [SerializeField]
        protected RectTransform draggingVerticalGameObject;

        [SerializeField] protected List<SwipeVerticalHorizontalMenu.ChildPageUI> verticalPages;
        [SerializeField] protected RectTransform initVerticalPosition;
        [SerializeField] protected int currentVerticalIndex, initVerticalIndex;
        [SerializeField, Range(0, 1)] protected float acceptVerticalThreshHold = 0.2f;
        protected Vector2 beforeDraggingVerticalPosition, expectDestinationVerticalPosition;
        protected Sequence verticalSequence;

        [Serializable]
        public class ChildPageUI
        {
            public RectTransform rectTransform;
            public UnityEvent onSelectEvent;
            public UnityEvent onDeselectEvent;

            public void OnSelect()
            {
                onSelectEvent.Invoke();
            }

            public void OnDeselect()
            {
                onDeselectEvent.Invoke();
            }
        }


        protected void OnEnable()
        {
            verticalPages = verticalPages.OrderBy(o => o.rectTransform.anchoredPosition.y).ToList();

            draggingVerticalGameObject.anchoredPosition = initVerticalPosition != null
                ? initVerticalPosition.anchoredPosition
                : draggingVerticalGameObject.anchoredPosition;
            
            currentVerticalIndex = initVerticalIndex;
            verticalPages[initVerticalIndex].OnSelect();

            expectDestinationVerticalPosition = beforeDraggingVerticalPosition = verticalPages[initVerticalIndex].rectTransform.anchoredPosition;

            verticalSequence = DOTween.Sequence();
            
            SmoothMoveTo(draggingVerticalGameObject, expectDestinationVerticalPosition, movingDuration, verticalPages[initVerticalIndex].onSelectEvent);

        }


        public void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log("Pointer down " + eventData.position + " Vertical " + beforeDraggingVerticalPosition);

            beforeDraggingVerticalPosition = draggingVerticalGameObject.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("On Drag " + eventData.position + " Vertical " + beforeDraggingVerticalPosition +
            //          " Horizontal " + beforeDraggingHorizontalPosition);
            
            float yDifference = eventData.position.y - eventData.pressPosition.y;
            draggingVerticalGameObject.anchoredPosition = new Vector2(0, yDifference) + beforeDraggingVerticalPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //Debug.Log("EndDrag "+ eventData.anchoredPosition + " - "+ eventData.pressPosition);
            
            float percentage = (eventData.position.y - eventData.pressPosition.y) / Screen.width;
            if (Mathf.Abs(percentage) >= acceptVerticalThreshHold)
            {
                Vector2 newLocation = expectDestinationVerticalPosition;
                if (percentage > 0 && currentVerticalIndex < verticalPages.Count - 1)
                {
                    newLocation += verticalPages[currentVerticalIndex + 1].rectTransform.anchoredPosition -
                                   verticalPages[currentVerticalIndex].rectTransform.anchoredPosition;

                    verticalPages[currentVerticalIndex].OnDeselect();
                    currentVerticalIndex++;
                    SmoothMoveTo(draggingVerticalGameObject, newLocation, movingDuration,
                        verticalPages[currentVerticalIndex].onSelectEvent);
                }
                else if (percentage < 0 && currentVerticalIndex >= 1)
                {
                    newLocation += verticalPages[currentVerticalIndex - 1].rectTransform.anchoredPosition -
                                   verticalPages[currentVerticalIndex].rectTransform.anchoredPosition;

                    verticalPages[currentVerticalIndex].OnDeselect();
                    currentVerticalIndex--;
                    SmoothMoveTo(draggingVerticalGameObject, newLocation, movingDuration,
                        verticalPages[currentVerticalIndex].onSelectEvent);
                }
                else
                {
                    SmoothMoveTo(draggingVerticalGameObject, newLocation, movingDuration, new UnityEvent());
                }
                //StartCoroutine(SmoothMove(draggingVerticalGameObject, draggingVerticalGameObject.anchoredPosition, newLocation, movingDuration));

                expectDestinationVerticalPosition = newLocation;
                //Debug.Log("Goto step " + currentVerticalIndex + " Position " + beforeDraggingVerticalPosition);
            }
            else
            {
                //StartCoroutine(SmoothMove(draggingVerticalGameObject,draggingVerticalGameObject.anchoredPosition, _beforeDraggingVerticalPosition, movingDuration));
                //Debug.Log("Before Back to beginning Position" + beforeDraggingVerticalPosition);

                SmoothMoveTo(draggingVerticalGameObject, expectDestinationVerticalPosition, movingDuration,
                    new UnityEvent());
                //Debug.Log("Back to beginning Position" + beforeDraggingVerticalPosition);
            }
        }

        private void SmoothMoveTo(RectTransform movingObject, Vector3 endPos, float seconds, UnityEvent onSelect)
        {
            verticalSequence.Kill();
            verticalSequence = DOTween.Sequence();
            verticalSequence.Append(movingObject.DOAnchorPos(endPos, seconds).SetEase(ease)
                .OnComplete(onSelect.Invoke));
            verticalSequence.Play();
        
        }
    }
}