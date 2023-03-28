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
    [Header("Animation Properties")] [SerializeField]
    protected float movingDuration = 0.5f;

    [SerializeField] protected Ease ease = Ease.InCubic;
    protected bool isDraggingHorizontalNorVertical;
    


    [Header("Horizontal Properties")] [SerializeField]
    protected RectTransform draggingHorizontalGameObject;
    [SerializeField] protected List<ChildPageUI> horizontalPages;
    [SerializeField] protected RectTransform initHorizontalTransform;
    [SerializeField] protected int currentHorizontalIndex, initHorizontalIndex;
    [SerializeField, Range(0, 1)] protected float acceptHorizontalThreshHold = 0.2f;
    protected Vector2 beforeDraggingHorizontalPosition, expectDestinationHorizontalPosition;
    protected Sequence horizontalSequence = DOTween.Sequence();

    [Header("Vertical Properties")] [SerializeField]
    protected RectTransform draggingVerticalGameObject;

    [SerializeField] protected List<ChildPageUI> verticalPages;
    [SerializeField] protected RectTransform initVerticalTransform;
    [SerializeField] protected int currentVerticalIndex, initVerticalIndex;
    [SerializeField, Range(0, 1)] protected float acceptVerticalThreshHold = 0.2f;
    protected Vector2 beforeDraggingVerticalPosition, expectDestinationVerticalPosition;
    protected Sequence verticalSequence = DOTween.Sequence();
    
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
        horizontalPages = horizontalPages.OrderBy(o => o.rectTransform.anchoredPosition.x).ToList();
        verticalPages = verticalPages.OrderBy(o => o.rectTransform.anchoredPosition.y).ToList();

        draggingVerticalGameObject.anchoredPosition = initVerticalTransform
            ? initVerticalTransform.anchoredPosition
            : draggingVerticalGameObject.anchoredPosition;
        draggingHorizontalGameObject.anchoredPosition = initHorizontalTransform
            ? initHorizontalTransform.anchoredPosition
            : draggingHorizontalGameObject.anchoredPosition;

        //Vector3 verticalDestination = verticalPages[currentVerticalStepIndex].rectTransform.anchoredPosition;
        //Vector3 horizontalDestination = horizontalPages[currentHorizontalStepIndex].rectTransform.anchoredPosition;

        currentHorizontalIndex = initHorizontalIndex;
        currentVerticalIndex = initVerticalIndex;
        horizontalPages[initHorizontalIndex].OnSelect();
        verticalPages[initVerticalIndex].OnSelect();

        draggingHorizontalGameObject.anchoredPosition = expectDestinationHorizontalPosition = beforeDraggingHorizontalPosition = horizontalPages[initHorizontalIndex].rectTransform.anchoredPosition;
        draggingVerticalGameObject.anchoredPosition = expectDestinationVerticalPosition = beforeDraggingVerticalPosition = verticalPages[initVerticalIndex].rectTransform.anchoredPosition;

        horizontalSequence = DOTween.Sequence();
        verticalSequence = DOTween.Sequence();
        //StartCoroutine(SmoothMove(draggingHorizontalGameObject, draggingHorizontalGameObject.anchoredPosition, _beforeDraggingHorizontalPosition, movingDuration));
        //StartCoroutine(SmoothMove(draggingVerticalGameObject, draggingVerticalGameObject.anchoredPosition, verticalDestination , movingDuration));
        //SmoothMoveTo(draggingVerticalGameObject, verticalDestination, movingDuration, verticalPages[currentVerticalStepIndex].onSelectEvent);
        //SmoothMoveTo(draggingHorizontalGameObject, horizontalDestination, movingDuration, horizontalPages[currentHorizontalStepIndex].onSelectEvent);
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
                                horizontalPages[currentHorizontalIndex - 1].rectTransform.anchoredPosition); // There for this is backward

                horizontalPages[currentHorizontalIndex].OnDeselect();
                currentHorizontalIndex--;
                SmoothMoveTo(draggingHorizontalGameObject, newLocation, movingDuration, horizontalPages[currentHorizontalIndex].onSelectEvent);
            }
            else if (percentage < 0 && currentHorizontalIndex < horizontalPages.Count - 1)
            {
                newLocation += (horizontalPages[currentHorizontalIndex].rectTransform.anchoredPosition -
                                horizontalPages[currentHorizontalIndex + 1].rectTransform.anchoredPosition); //there for this is backward

                horizontalPages[currentHorizontalIndex].OnDeselect();
                currentHorizontalIndex++;
                SmoothMoveTo(draggingHorizontalGameObject, newLocation, movingDuration, horizontalPages[currentHorizontalIndex].onSelectEvent);
            }
            else
            {
                SmoothMoveTo(draggingHorizontalGameObject, newLocation, movingDuration,new UnityEvent());

            } 

            //StartCoroutine(SmoothMove(draggingHorizontalGameObject, draggingHorizontalGameObject.anchoredPosition, newLocation, movingDuration));
            
            expectDestinationHorizontalPosition = newLocation;
            //Debug.Log("Goto step " + currentHorizontalIndex + " Position " + beforeDraggingHorizontalPosition);
        }
        else
        {
            //StartCoroutine(SmoothMove(draggingHorizontalGameObject,draggingHorizontalGameObject.anchoredPosition, _beforeDraggingHorizontalPosition, movingDuration));
            //Debug.Log("Before Back to beginning Position" + beforeDraggingHorizontalPosition);

            SmoothMoveTo(draggingHorizontalGameObject, expectDestinationHorizontalPosition, movingDuration, new UnityEvent());

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
                SmoothMoveTo(draggingVerticalGameObject, newLocation, movingDuration,verticalPages[currentVerticalIndex].onSelectEvent);
            }
            else if (percentage < 0 && currentVerticalIndex >= 1)
            {
                newLocation += verticalPages[currentVerticalIndex - 1].rectTransform.anchoredPosition -
                               verticalPages[currentVerticalIndex].rectTransform.anchoredPosition;

                verticalPages[currentVerticalIndex].OnDeselect();
                currentVerticalIndex--;
                SmoothMoveTo(draggingVerticalGameObject, newLocation, movingDuration,verticalPages[currentVerticalIndex].onSelectEvent);
            }
            else
            {
                SmoothMoveTo(draggingVerticalGameObject, newLocation, movingDuration,new UnityEvent());
            } 
            //StartCoroutine(SmoothMove(draggingVerticalGameObject, draggingVerticalGameObject.anchoredPosition, newLocation, movingDuration));

            expectDestinationVerticalPosition = newLocation;
            //Debug.Log("Goto step " + currentVerticalIndex + " Position " + beforeDraggingVerticalPosition);
        }
        else
        {
            //StartCoroutine(SmoothMove(draggingVerticalGameObject,draggingVerticalGameObject.anchoredPosition, _beforeDraggingVerticalPosition, movingDuration));
            //Debug.Log("Before Back to beginning Position" + beforeDraggingVerticalPosition);

            SmoothMoveTo(draggingVerticalGameObject, expectDestinationVerticalPosition, movingDuration, new UnityEvent());
            //Debug.Log("Back to beginning Position" + beforeDraggingVerticalPosition);
        }
    }

    #region Horizontal Child Page

    public void OnPointerDownHorizontalChildPage(PointerEventData eventData)
    {
        beforeDraggingVerticalPosition = draggingVerticalGameObject.anchoredPosition;
        beforeDraggingHorizontalPosition = draggingHorizontalGameObject.anchoredPosition;
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
    
    private void SmoothMoveTo(RectTransform movingObject, Vector3 endPos, float seconds, UnityEvent onSelect)
    {
        if (movingObject == draggingHorizontalGameObject)
        {
            horizontalSequence.Kill();
            horizontalSequence = DOTween.Sequence();
            horizontalSequence.Append( movingObject.DOAnchorPos(endPos, seconds).SetEase(ease).OnComplete(onSelect.Invoke));
            horizontalSequence.Play();
        }
        else if (movingObject == draggingVerticalGameObject)
        {
            verticalSequence.Kill();
            verticalSequence = DOTween.Sequence();
            verticalSequence.Append( movingObject.DOAnchorPos(endPos, seconds).SetEase(ease).OnComplete(onSelect.Invoke));
            verticalSequence.Play();
        }        
    }

    
    /*
    IEnumerator SmoothMove(RectTransform movingObject, Vector3 startPos, Vector3 endPos, float seconds){
        if (_isMoving) {
            yield return new WaitForSeconds(seconds); // exit coroutine if already moving
        }

        _isMoving = true;
        
        float t = 0f;
        while(t <= 1.0){
            t += Time.deltaTime / seconds;
            movingObject.anchoredPosition = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        _isMoving = false;
    }
    */

}