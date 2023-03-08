using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
public class SwipeGridStepsPage : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("Properties")]
    [SerializeField] private float movingDuration = 0.5f;
    private bool _isDraggingHorizontalNorVertical;
    private bool _isFirstTimeDragging = true;
    
    [Header("Horizontal Properties")] 
    [SerializeField] private RectTransform draggingHorizontalGameObject; 
    [SerializeField] private List<RectTransform> stepHorizontalTransforms;
    [SerializeField] private int _currentHorizontalStepIndex ;
    [SerializeField] private int _initHorizontalStepIndex = 0;
    [SerializeField] private float acceptHorizontalThreshHold = 0.2f;
    private Vector2 _beforeDraggingHorizontalPosition;

    
    
    [Header("Vertical Properties")]
    [SerializeField] private RectTransform draggingVerticalGameObject;
    [SerializeField] private List<RectTransform> stepVerticalTransforms ;
    [SerializeField] private int _currentVerticalStepIndex ;
    [SerializeField] private int _initVerticalStepIndex=0 ;
    [SerializeField] private float acceptVerticalThreshHold = 0.2f;
    private Vector2 _beforeDraggingVerticalPosition;
    
    private void Start()
    {
        stepHorizontalTransforms = stepHorizontalTransforms.OrderBy(o => o.anchoredPosition.x).Reverse().ToList();
        stepVerticalTransforms = stepVerticalTransforms.OrderBy(o => o.anchoredPosition.y).ToList();
        
        draggingVerticalGameObject.anchoredPosition = stepVerticalTransforms[_initVerticalStepIndex].anchoredPosition;
        //draggingHorizontalGameObject.anchoredPosition = stepHorizontalTransforms[_initHorizontalStepIndex].anchoredPosition;
        
        _beforeDraggingVerticalPosition = stepVerticalTransforms[_currentVerticalStepIndex].anchoredPosition;
        _beforeDraggingHorizontalPosition = draggingHorizontalGameObject.anchoredPosition;
        
        //StartCoroutine(SmoothMove(draggingHorizontalGameObject, draggingHorizontalGameObject.anchoredPosition, _beforeDraggingHorizontalPosition, movingDuration));
        StartCoroutine(SmoothMove(draggingVerticalGameObject, draggingVerticalGameObject.anchoredPosition, _beforeDraggingVerticalPosition, movingDuration));

        
    }
    
    
    
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag "+ eventData.anchoredPosition + " - "+ eventData.pressPosition);

        float xDifference = ( eventData.position.x - eventData.pressPosition.x);
        float yDifference = eventData.position.y - eventData.pressPosition.y;
        if (_isFirstTimeDragging)
        {
            _isFirstTimeDragging = false;
            _isDraggingHorizontalNorVertical = Mathf.Abs( xDifference) >= Mathf.Abs( yDifference) ;
        }
        else
        {
            if (_isDraggingHorizontalNorVertical)
            {
                draggingHorizontalGameObject.anchoredPosition = _beforeDraggingHorizontalPosition + new Vector2(xDifference, 0);
            }
            else
            {
                draggingVerticalGameObject.anchoredPosition = _beforeDraggingVerticalPosition + new Vector2(0,yDifference );
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("EndDrag "+ eventData.anchoredPosition + " - "+ eventData.pressPosition);
        _isFirstTimeDragging = true;
        if (_isDraggingHorizontalNorVertical)
        {
            OnHorizontalEndDrag(eventData);
        }
        else
        {
            OnVerticalEndDrag(eventData);
        }
        
    }

    private void OnHorizontalEndDrag(PointerEventData eventData)
    {
        float percentage = (eventData.position.x - eventData.pressPosition.x) / Screen.height;
        if(Mathf.Abs(percentage) >= acceptHorizontalThreshHold){
            Vector2 newLocation = _beforeDraggingHorizontalPosition;
            if(percentage > 0  && _currentHorizontalStepIndex >= 1)
            {
                newLocation += (stepHorizontalTransforms[_currentHorizontalStepIndex - 1].anchoredPosition -
                               stepHorizontalTransforms[_currentHorizontalStepIndex].anchoredPosition);
                _currentHorizontalStepIndex--;
            }
            else if(percentage < 0 && _currentHorizontalStepIndex < stepHorizontalTransforms.Count-1){
                newLocation += (stepHorizontalTransforms[_currentHorizontalStepIndex + 1].anchoredPosition -
                               stepHorizontalTransforms[_currentHorizontalStepIndex].anchoredPosition);
                _currentHorizontalStepIndex++;
            }
        
            
            StartCoroutine(SmoothMove(draggingHorizontalGameObject, draggingHorizontalGameObject.anchoredPosition, newLocation, movingDuration));
            _beforeDraggingHorizontalPosition = newLocation;
            Debug.Log("Goto step "+ _currentHorizontalStepIndex +" Position "+ _beforeDraggingHorizontalPosition );
        }
        else{
            StartCoroutine(SmoothMove(draggingHorizontalGameObject,draggingHorizontalGameObject.anchoredPosition, _beforeDraggingHorizontalPosition, movingDuration));
            Debug.Log("Back to beginning Position"+ _beforeDraggingHorizontalPosition );
        }
        
        foreach (var step in stepHorizontalTransforms)
        {
            Debug.Log(step.name+" "+step.anchoredPosition);
        }
    }
    
    
    private void OnVerticalEndDrag(PointerEventData eventData)
    {
        float percentage = (eventData.position.y - eventData.pressPosition.y) / Screen.width;
        if(Mathf.Abs(percentage) >= acceptVerticalThreshHold){
            Vector2 newLocation = _beforeDraggingVerticalPosition;
            if(percentage > 0 && _currentVerticalStepIndex < stepVerticalTransforms.Count-1){
                newLocation += stepVerticalTransforms[_currentVerticalStepIndex+1].anchoredPosition - stepVerticalTransforms[_currentVerticalStepIndex].anchoredPosition ;
                _currentVerticalStepIndex++;
            }
            else if(percentage < 0 && _currentVerticalStepIndex >= 1){
                newLocation += stepVerticalTransforms[_currentVerticalStepIndex-1].anchoredPosition - stepVerticalTransforms[_currentVerticalStepIndex].anchoredPosition ;
                _currentVerticalStepIndex--;
            }
            
            StartCoroutine(SmoothMove(draggingVerticalGameObject, draggingVerticalGameObject.anchoredPosition, newLocation, movingDuration));
            _beforeDraggingVerticalPosition = newLocation;
            Debug.Log("Goto step "+ _currentVerticalStepIndex +" Position "+ _beforeDraggingVerticalPosition );
        }
        else{
            StartCoroutine(SmoothMove(draggingVerticalGameObject,draggingVerticalGameObject.anchoredPosition, _beforeDraggingVerticalPosition, movingDuration));
            Debug.Log("Back to beginning Position"+ _beforeDraggingVerticalPosition );
        }
    }
    
    IEnumerator SmoothMove(RectTransform movingObject, Vector3 startPos, Vector3 endPos, float seconds){
        float t = 0f;
        while(t <= 1.0){
            t += Time.deltaTime / seconds;
            movingObject.anchoredPosition = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

}
