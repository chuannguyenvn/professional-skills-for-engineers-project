using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;



public class SwipeVerticalHorizontalMenu : MonoBehaviour, IPointerDownHandler, IBeginDragHandler,IDragHandler, IEndDragHandler
{
    [Header("Animation Properties")] 
    [SerializeField] protected float movingDuration = 0.5f;
    [SerializeField] protected Ease ease = Ease.OutCubic;
    protected bool isDraggingHorizontalNorVertical;
    
    
    [Header("Horizontal Properties")] 
    [SerializeField] protected RectTransform draggingHorizontalGameObject;
    [SerializeField] protected RectTransform initHorizontalPosition;
    [SerializeField] public List<ChildPageUI> horizontalPages;
    [SerializeField] protected int initHorizontalIndex, exitHorizontalIndex;
    [SerializeField, Range(0, 1)] protected float acceptHorizontalThreshHold = 0.2f;
    [SerializeField] protected int currentHorizontalIndex;
    protected Vector2 beforeDraggingHorizontalPosition, expectDestinationHorizontalPosition;
    protected Sequence horizontalSequence;

    [Header("Vertical Properties")] 
    [SerializeField] protected RectTransform draggingVerticalGameObject;
    [SerializeField] protected RectTransform initVerticalPosition;
    [SerializeField] public List<ChildPageUI> verticalPages;
    [SerializeField] protected int initVerticalIndex, exitVerticalIndex;
    [SerializeField, Range(0, 1)] protected float acceptVerticalThreshHold = 0.2f;
    [SerializeField] protected int currentVerticalIndex;
    protected Vector2 beforeDraggingVerticalPosition, expectDestinationVerticalPosition;
    protected Sequence verticalSequence;
    
    [Serializable]
    public class ChildPageUI
    {
        public RectTransform rectTransform;
        public UnityEvent onSelectEvent;
        public UnityEvent onEnterCompleteEvent;
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
    
    public virtual void Show()
    {
        horizontalPages = horizontalPages.OrderBy(o => o.rectTransform.anchoredPosition.x).ToList();
        verticalPages = verticalPages.OrderBy(o => o.rectTransform.anchoredPosition.y).ToList();
        
        draggingVerticalGameObject.anchoredPosition = initVerticalPosition != null
            ? initVerticalPosition.anchoredPosition
            : draggingVerticalGameObject.anchoredPosition;
        draggingHorizontalGameObject.anchoredPosition = initHorizontalPosition != null
            ? initHorizontalPosition.anchoredPosition
            : draggingHorizontalGameObject.anchoredPosition;


        currentHorizontalIndex = initHorizontalIndex;
        currentVerticalIndex = initVerticalIndex;
        horizontalPages[initHorizontalIndex].OnSelect();
        verticalPages[initVerticalIndex].OnSelect();

        expectDestinationHorizontalPosition = beforeDraggingHorizontalPosition = horizontalPages[initHorizontalIndex].rectTransform.anchoredPosition;
        expectDestinationVerticalPosition = beforeDraggingVerticalPosition = verticalPages[initVerticalIndex].rectTransform.anchoredPosition;

        horizontalSequence = DOTween.Sequence();
        verticalSequence = DOTween.Sequence();
        
        
        SmoothMoveTo(draggingVerticalGameObject, expectDestinationVerticalPosition, movingDuration, verticalPages[initVerticalIndex].onEnterCompleteEvent );
        SmoothMoveTo(draggingHorizontalGameObject, expectDestinationHorizontalPosition, movingDuration, horizontalPages[initHorizontalIndex].onEnterCompleteEvent);
    }

    public void Hide()
    {
        expectDestinationHorizontalPosition = beforeDraggingHorizontalPosition = horizontalPages[exitHorizontalIndex].rectTransform.anchoredPosition;
        expectDestinationVerticalPosition = beforeDraggingVerticalPosition = verticalPages[exitVerticalIndex].rectTransform.anchoredPosition;

        SmoothMoveTo(draggingVerticalGameObject, expectDestinationVerticalPosition, movingDuration, verticalPages[exitVerticalIndex].onEnterCompleteEvent);
        SmoothMoveTo(draggingHorizontalGameObject, expectDestinationHorizontalPosition, movingDuration, horizontalPages[exitHorizontalIndex].onEnterCompleteEvent);
    }

    public void SetToHorizontalPage(int index)
    {
        if(index < 0 || index >= horizontalPages.Count || index == currentHorizontalIndex) return;
        
        horizontalPages[currentHorizontalIndex].OnDeselect();
        Debug.Log(expectDestinationVerticalPosition +" "+ (horizontalPages[currentHorizontalIndex].rectTransform.anchoredPosition -
                                                           horizontalPages[index].rectTransform.anchoredPosition));

        expectDestinationHorizontalPosition += (horizontalPages[currentHorizontalIndex].rectTransform.anchoredPosition -
                                              horizontalPages[index].rectTransform.anchoredPosition); // Therefore this is backward
        currentHorizontalIndex = index;
        horizontalPages[index].OnSelect();        
        SmoothMoveTo(draggingHorizontalGameObject, expectDestinationHorizontalPosition, movingDuration, horizontalPages[index].onEnterCompleteEvent);

    }

    public void SetToVerticalPage(int index)
    {
        if(index < 0 || index >= verticalPages.Count || index == currentVerticalIndex) return;
        
        verticalPages[currentVerticalIndex].OnDeselect();
        expectDestinationVerticalPosition += verticalPages[index].rectTransform.anchoredPosition -
                       verticalPages[currentVerticalIndex].rectTransform.anchoredPosition;
        currentVerticalIndex = index;
        verticalPages[index].OnSelect();
        SmoothMoveTo(draggingVerticalGameObject, expectDestinationVerticalPosition, movingDuration, verticalPages[index].onEnterCompleteEvent);
        
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Pointer down " + eventData.position + " Vertical " + beforeDraggingVerticalPosition +
        //          " Horizontal " + beforeDraggingHorizontalPosition);
        
        beforeDraggingVerticalPosition = draggingVerticalGameObject.anchoredPosition;
        beforeDraggingHorizontalPosition = draggingHorizontalGameObject.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Begin Drag " + eventData.position + " Vertical " + beforeDraggingVerticalPosition +
        //          " Horizontal " + beforeDraggingHorizontalPosition);
        float xDifference = eventData.position.x - eventData.pressPosition.x;
        float yDifference = eventData.position.y - eventData.pressPosition.y;
        
        isDraggingHorizontalNorVertical = Mathf.Abs(xDifference) >= Mathf.Abs(yDifference);
    }
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("On Drag " + eventData.position + " Vertical " + beforeDraggingVerticalPosition +
        //          " Horizontal " + beforeDraggingHorizontalPosition);
        if (isDraggingHorizontalNorVertical)
        {
            float xDifference = eventData.position.x - eventData.pressPosition.x;
            draggingHorizontalGameObject.anchoredPosition = new Vector2(xDifference, 0) + beforeDraggingHorizontalPosition;
        }
        else
        {
            float yDifference = eventData.position.y - eventData.pressPosition.y;
            draggingVerticalGameObject.anchoredPosition =  new Vector2(0, yDifference) + beforeDraggingVerticalPosition ;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("EndDrag "+ eventData.anchoredPosition + " - "+ eventData.pressPosition);
        if (isDraggingHorizontalNorVertical)
        {
            OnHorizontalEndDrag(eventData);
        }
        else
        {
            OnVerticalEndDrag(eventData);
        }
    }

    // The Index is from right to left , right is 0 and left is count-1
    protected void OnHorizontalEndDrag(PointerEventData eventData)
    {
        float percentage = (eventData.position.x - eventData.pressPosition.x) / Screen.height;
        if (Mathf.Abs(percentage) >= acceptHorizontalThreshHold)
        {
            Vector2 newLocation = expectDestinationHorizontalPosition;
            if (percentage > 0 && currentHorizontalIndex >= 1)
            {
                newLocation += (horizontalPages[currentHorizontalIndex].rectTransform.anchoredPosition -
                                horizontalPages[currentHorizontalIndex - 1].rectTransform.anchoredPosition); // Therefore this is backward

                horizontalPages[currentHorizontalIndex].OnDeselect();
                currentHorizontalIndex--;
                horizontalPages[currentHorizontalIndex].OnSelect();
                SmoothMoveTo(draggingHorizontalGameObject, newLocation, movingDuration, horizontalPages[currentHorizontalIndex].onEnterCompleteEvent);
            }
            else if (percentage < 0 && currentHorizontalIndex < horizontalPages.Count - 1)
            {
                newLocation += (horizontalPages[currentHorizontalIndex].rectTransform.anchoredPosition -
                                horizontalPages[currentHorizontalIndex + 1].rectTransform.anchoredPosition); //Therefore this is backward

                horizontalPages[currentHorizontalIndex].OnDeselect();
                currentHorizontalIndex++;
                horizontalPages[currentHorizontalIndex].OnSelect();
                SmoothMoveTo(draggingHorizontalGameObject, newLocation, movingDuration, horizontalPages[currentHorizontalIndex].onEnterCompleteEvent);
            }
            else
            {
                SmoothMoveTo(draggingHorizontalGameObject, newLocation, movingDuration);

            } 

            //StartCoroutine(SmoothMove(draggingHorizontalGameObject, draggingHorizontalGameObject.anchoredPosition, newLocation, movingDuration));
            
            expectDestinationHorizontalPosition = newLocation;
            //Debug.Log("Goto step " + currentHorizontalIndex + " Position " + beforeDraggingHorizontalPosition);
        }
        else
        {
            //StartCoroutine(SmoothMove(draggingHorizontalGameObject,draggingHorizontalGameObject.anchoredPosition, _beforeDraggingHorizontalPosition, movingDuration));
            //Debug.Log("Before Back to beginning Position" + beforeDraggingHorizontalPosition);

            SmoothMoveTo(draggingHorizontalGameObject, expectDestinationHorizontalPosition, movingDuration);

            //Debug.Log("Back to beginning Position" + beforeDraggingHorizontalPosition);
        }
    }


    protected void OnVerticalEndDrag(PointerEventData eventData)
    {
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
                verticalPages[currentVerticalIndex].OnSelect();
                SmoothMoveTo(draggingVerticalGameObject, newLocation, movingDuration, verticalPages[currentVerticalIndex].onEnterCompleteEvent);
            }
            else if (percentage < 0 && currentVerticalIndex >= 1)
            {
                newLocation += verticalPages[currentVerticalIndex - 1].rectTransform.anchoredPosition -
                               verticalPages[currentVerticalIndex].rectTransform.anchoredPosition;

                verticalPages[currentVerticalIndex].OnDeselect();
                currentVerticalIndex--;
                verticalPages[currentVerticalIndex].OnSelect();
                SmoothMoveTo(draggingVerticalGameObject, newLocation, movingDuration, verticalPages[currentVerticalIndex].onEnterCompleteEvent);
            }
            else
            {
                SmoothMoveTo(draggingVerticalGameObject, newLocation, movingDuration);
            } 
            //StartCoroutine(SmoothMove(draggingVerticalGameObject, draggingVerticalGameObject.anchoredPosition, newLocation, movingDuration));

            expectDestinationVerticalPosition = newLocation;
            //Debug.Log("Goto step " + currentVerticalIndex + " Position " + beforeDraggingVerticalPosition);
        }
        else
        {
            //StartCoroutine(SmoothMove(draggingVerticalGameObject,draggingVerticalGameObject.anchoredPosition, _beforeDraggingVerticalPosition, movingDuration));
            //Debug.Log("Before Back to beginning Position" + beforeDraggingVerticalPosition);

            SmoothMoveTo(draggingVerticalGameObject, expectDestinationVerticalPosition, movingDuration);
            //Debug.Log("Back to beginning Position" + beforeDraggingVerticalPosition);
        }
    }

    #region Horizontal Child Page

    public void OnPointerDownHorizontalChildPage(PointerEventData eventData)
    {
        OnPointerDown(eventData);
    }

    public void OnBeginDragHorizontalChildPage(PointerEventData eventData)
    {
        isDraggingHorizontalNorVertical = true;
    }
    
    public void OnDragHorizontalChildPage(PointerEventData eventData)
    {
        float xDifference = eventData.position.x - eventData.pressPosition.x;
        draggingHorizontalGameObject.anchoredPosition = beforeDraggingHorizontalPosition + new Vector2(xDifference, 0);
    }

    public void OnEndDragHorizontalChildPage(PointerEventData eventData)
    {
        OnHorizontalEndDrag(eventData);
    }
    
    #endregion
    
    protected void SmoothMoveTo(RectTransform movingObject, Vector3 endPos, float seconds, UnityEvent onComplete = null)
    {
        if (movingObject == draggingHorizontalGameObject)
        {
            horizontalSequence.Kill();
            horizontalSequence = DOTween.Sequence();
            if (onComplete != null)
                horizontalSequence.Append(movingObject.DOAnchorPos(endPos, seconds).SetEase(ease))
                    .OnComplete(onComplete.Invoke);
            else 
                horizontalSequence.Append(movingObject.DOAnchorPos(endPos, seconds).SetEase(ease));
            horizontalSequence.Play();
        }
        else if (movingObject == draggingVerticalGameObject)
        {
            verticalSequence.Kill();
            verticalSequence = DOTween.Sequence();
            if (onComplete != null)
                verticalSequence.Append(movingObject.DOAnchorPos(endPos, seconds).SetEase(ease))
                    .OnComplete(onComplete.Invoke);
            else
                verticalSequence.Append(movingObject.DOAnchorPos(endPos, seconds).SetEase(ease));
                   
            verticalSequence.Play();
        }        
    }
    
}