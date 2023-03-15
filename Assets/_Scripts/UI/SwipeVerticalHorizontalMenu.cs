using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;



public class SwipeVerticalHorizontalMenu : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("Animation Properties")] [SerializeField]
    protected float movingDuration = 0.5f;

    [SerializeField] protected Ease ease = Ease.InCubic;
    protected bool isDraggingHorizontalNorVertical;
    protected bool isFirstTimeDragging = true;
    protected bool isMoving = false;


    [Header("Horizontal Properties")] [SerializeField]
    protected RectTransform draggingHorizontalGameObject;

    [SerializeField] protected List<ChildPageUI> horizontalPages;
    [SerializeField] protected RectTransform initHorizontalTransform;

    [SerializeField] protected int currentHorizontalIndex, initHorizontalIndex;
    [SerializeField, Range(0, 1)] protected float acceptHorizontalThreshHold = 0.2f;
    protected Vector2 beforeDraggingHorizontalPosition;


    [Header("Vertical Properties")] [SerializeField]
    protected RectTransform draggingVerticalGameObject;

    [SerializeField] protected List<ChildPageUI> verticalPages;
    [SerializeField] protected RectTransform initVerticalTransform;
    [SerializeField] protected int currentVerticalIndex, initVerticalIndex;
    [SerializeField, Range(0, 1)] protected float acceptVerticalThreshHold = 0.2f;
    protected Vector2 beforeDraggingVerticalPosition;

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

        //beforeDraggingHorizontalPosition = horizontalPages[initHorizontalIndex].rectTransform.anchoredPosition;
        //beforeDraggingVerticalPosition = verticalPages[initVerticalIndex].rectTransform.anchoredPosition;

        //StartCoroutine(SmoothMove(draggingHorizontalGameObject, draggingHorizontalGameObject.anchoredPosition, _beforeDraggingHorizontalPosition, movingDuration));
        //StartCoroutine(SmoothMove(draggingVerticalGameObject, draggingVerticalGameObject.anchoredPosition, verticalDestination , movingDuration));
        //SmoothMoveTo(draggingVerticalGameObject, verticalDestination, movingDuration, verticalPages[currentVerticalStepIndex].onSelectEvent);
        //SmoothMoveTo(draggingHorizontalGameObject, horizontalDestination, movingDuration, horizontalPages[currentHorizontalStepIndex].onSelectEvent);
    }

    

    public void OnDrag(PointerEventData eventData)
    {

        //Debug.Log("Drag "+ eventData.anchoredPosition + " - "+ eventData.pressPosition);
        float xDifference = eventData.position.x - eventData.pressPosition.x;
        float yDifference = eventData.position.y - eventData.pressPosition.y;
        if (isFirstTimeDragging)
        {
            isFirstTimeDragging = false;
            isDraggingHorizontalNorVertical = Mathf.Abs(xDifference) >= Mathf.Abs(yDifference);

            beforeDraggingVerticalPosition = draggingVerticalGameObject.anchoredPosition;
            beforeDraggingHorizontalPosition = draggingHorizontalGameObject.anchoredPosition;
        }

        if (isDraggingHorizontalNorVertical)
        {
            draggingHorizontalGameObject.anchoredPosition =
                beforeDraggingHorizontalPosition + new Vector2(xDifference, 0);
        }
        else
        {
            draggingVerticalGameObject.anchoredPosition = beforeDraggingVerticalPosition + new Vector2(0, yDifference);
        }
    }

    public void OnDragHorizontalChildPage(PointerEventData eventData)
    {
        float xDifference = eventData.position.x - eventData.pressPosition.x;

        if (isFirstTimeDragging)
        {
            isFirstTimeDragging = false;
            isDraggingHorizontalNorVertical = true;
            beforeDraggingVerticalPosition = draggingVerticalGameObject.anchoredPosition;
            beforeDraggingHorizontalPosition = draggingHorizontalGameObject.anchoredPosition;
        }

        draggingHorizontalGameObject.anchoredPosition = beforeDraggingHorizontalPosition + new Vector2(xDifference, 0);
    }

    public void OnEndDragHorizontalChildPage(PointerEventData eventData)
    {
        isFirstTimeDragging = true;
        OnHorizontalEndDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("EndDrag "+ eventData.anchoredPosition + " - "+ eventData.pressPosition);
        isFirstTimeDragging = true;
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
            Vector2 newLocation = beforeDraggingHorizontalPosition;
            if (percentage > 0 && currentHorizontalIndex >= 1)
            {
                newLocation += (horizontalPages[currentHorizontalIndex].rectTransform.anchoredPosition -
                                horizontalPages[currentHorizontalIndex - 1].rectTransform.anchoredPosition); // There for this is backward

                horizontalPages[currentHorizontalIndex].OnDeselect();
                currentHorizontalIndex--;
                
            }
            else if (percentage < 0 && currentHorizontalIndex < horizontalPages.Count - 1)
            {
                newLocation += (horizontalPages[currentHorizontalIndex].rectTransform.anchoredPosition -
                                horizontalPages[currentHorizontalIndex + 1].rectTransform.anchoredPosition); //there for this is backward

                horizontalPages[currentHorizontalIndex].OnDeselect();
                currentHorizontalIndex++;
                
            }


            //StartCoroutine(SmoothMove(draggingHorizontalGameObject, draggingHorizontalGameObject.anchoredPosition, newLocation, movingDuration));
            SmoothMoveTo(draggingHorizontalGameObject, newLocation, movingDuration, horizontalPages[currentHorizontalIndex].onSelectEvent);

            beforeDraggingHorizontalPosition = newLocation;
            Debug.Log("Goto step " + currentHorizontalIndex + " Position " + beforeDraggingHorizontalPosition);
        }
        else
        {
            //StartCoroutine(SmoothMove(draggingHorizontalGameObject,draggingHorizontalGameObject.anchoredPosition, _beforeDraggingHorizontalPosition, movingDuration));
            SmoothMoveTo(draggingHorizontalGameObject, beforeDraggingHorizontalPosition, movingDuration, new UnityEvent());

            Debug.Log("Back to beginning Position" + beforeDraggingHorizontalPosition);
        }
    }


    protected void OnVerticalEndDrag(PointerEventData eventData)
    {
        float percentage = (eventData.position.y - eventData.pressPosition.y) / Screen.width;
        if (Mathf.Abs(percentage) >= acceptVerticalThreshHold)
        {
            Vector2 newLocation = beforeDraggingVerticalPosition;
            if (percentage > 0 && currentVerticalIndex < verticalPages.Count - 1)
            {
                newLocation += verticalPages[currentVerticalIndex + 1].rectTransform.anchoredPosition -
                               verticalPages[currentVerticalIndex].rectTransform.anchoredPosition;

                verticalPages[currentVerticalIndex].OnDeselect();
                currentVerticalIndex++;
                
            }
            else if (percentage < 0 && currentVerticalIndex >= 1)
            {
                newLocation += verticalPages[currentVerticalIndex - 1].rectTransform.anchoredPosition -
                               verticalPages[currentVerticalIndex].rectTransform.anchoredPosition;

                verticalPages[currentVerticalIndex].OnDeselect();
                currentVerticalIndex--;
            }

            //StartCoroutine(SmoothMove(draggingVerticalGameObject, draggingVerticalGameObject.anchoredPosition, newLocation, movingDuration));
            SmoothMoveTo(draggingVerticalGameObject, newLocation, movingDuration,verticalPages[currentVerticalIndex].onSelectEvent);

            beforeDraggingVerticalPosition = newLocation;
            Debug.Log("Goto step " + currentVerticalIndex + " Position " + beforeDraggingVerticalPosition);
        }
        else
        {
            //StartCoroutine(SmoothMove(draggingVerticalGameObject,draggingVerticalGameObject.anchoredPosition, _beforeDraggingVerticalPosition, movingDuration));
            SmoothMoveTo(draggingHorizontalGameObject, beforeDraggingVerticalPosition, movingDuration, new UnityEvent());
            Debug.Log("Back to beginning Position" + beforeDraggingVerticalPosition);
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

    private void SmoothMoveTo(RectTransform movingObject, Vector3 endPos, float seconds, UnityEvent onSelect)
    {
        movingObject.DOAnchorPos(endPos, seconds).SetEase(ease).OnComplete(onSelect.Invoke);
    }
}