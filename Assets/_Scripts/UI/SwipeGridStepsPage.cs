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
    [SerializeField] private GameObject draggingHorizontalGameObject; 
    [SerializeField] private List<RectTransform> stepHorizontalTransforms;
    [SerializeField] private int _currentHorizontalStepIndex ;
    [SerializeField] private int _initHorizontalStepIndex = 0;
    [SerializeField] private float acceptHorizontalThreshHold = 0.2f;
    private Vector3 _beforeDraggingHorizontalPosition;

    
    
    [Header("Vertical Properties")]
    [SerializeField] private GameObject draggingVerticalGameObject;
    [SerializeField] private List<RectTransform> stepVerticalTransforms ;
    [SerializeField] private int _currentVerticalStepIndex ;
    [SerializeField] private int _initVerticalStepIndex=0 ;
    [SerializeField] private float acceptVerticalThreshHold = 0.2f;
    private Vector3 _beforeDraggingVerticalPosition;
    
    private void Start()
    {
        stepHorizontalTransforms = stepHorizontalTransforms.OrderBy(o => o.position.x).Reverse().ToList();
        stepVerticalTransforms = stepVerticalTransforms.OrderBy(o => o.position.y).ToList();

        
        draggingVerticalGameObject.transform.position = stepVerticalTransforms[_initVerticalStepIndex].position;
        draggingHorizontalGameObject.transform.position = stepHorizontalTransforms[_initHorizontalStepIndex].localPosition + stepVerticalTransforms[_initVerticalStepIndex].position;
        
        _beforeDraggingVerticalPosition = stepVerticalTransforms[_currentVerticalStepIndex].position;
        _beforeDraggingHorizontalPosition = stepHorizontalTransforms[_currentHorizontalStepIndex].localPosition + _beforeDraggingVerticalPosition;
        
        StartCoroutine(SmoothMove(draggingHorizontalGameObject, draggingHorizontalGameObject.transform.position, _beforeDraggingHorizontalPosition, movingDuration));
        StartCoroutine(SmoothMove(draggingVerticalGameObject, draggingVerticalGameObject.transform.position, _beforeDraggingVerticalPosition, movingDuration));

    }
    
    
    
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag "+ eventData.position + " - "+ eventData.pressPosition);

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
                draggingHorizontalGameObject.transform.position = _beforeDraggingHorizontalPosition + new Vector3(xDifference, 0,0);
            }
            else
            {
                draggingVerticalGameObject.transform.position = _beforeDraggingVerticalPosition + new Vector3(0,yDifference ,0);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("EndDrag "+ eventData.position + " - "+ eventData.pressPosition);
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
            Vector3 newLocation = _beforeDraggingHorizontalPosition;
            if(percentage > 0  && _currentHorizontalStepIndex >= 1){
                _currentHorizontalStepIndex--;
            }
            else if(percentage < 0 && _currentHorizontalStepIndex < stepHorizontalTransforms.Count-1){
                _currentHorizontalStepIndex++;
            }
        
            newLocation = stepHorizontalTransforms[_currentHorizontalStepIndex].localPosition + _beforeDraggingVerticalPosition;
            StartCoroutine(SmoothMove(draggingHorizontalGameObject, draggingHorizontalGameObject.transform.position, newLocation, movingDuration));
            _beforeDraggingHorizontalPosition = newLocation;
            Debug.Log("Goto step "+ _currentHorizontalStepIndex +" Position "+ _beforeDraggingHorizontalPosition );
        }
        else{
            StartCoroutine(SmoothMove(draggingHorizontalGameObject,draggingHorizontalGameObject.transform.position, _beforeDraggingHorizontalPosition, movingDuration));
            Debug.Log("Back to beginning Position"+ _beforeDraggingHorizontalPosition );
        }
    }
    
    
    private void OnVerticalEndDrag(PointerEventData eventData)
    {
        float percentage = (eventData.position.y - eventData.pressPosition.y) / Screen.width;
        if(Mathf.Abs(percentage) >= acceptVerticalThreshHold){
            Vector3 newLocation = _beforeDraggingVerticalPosition;
            if(percentage > 0 && _currentVerticalStepIndex < stepVerticalTransforms.Count-1){
                _currentVerticalStepIndex++;
            }
            else if(percentage < 0 && _currentVerticalStepIndex >= 1){
                _currentVerticalStepIndex--;
            }
        
            newLocation = stepVerticalTransforms[_currentVerticalStepIndex].position ;
            StartCoroutine(SmoothMove(draggingVerticalGameObject, draggingVerticalGameObject.transform.position, newLocation, movingDuration));
            _beforeDraggingHorizontalPosition += newLocation - _beforeDraggingVerticalPosition;
            _beforeDraggingVerticalPosition = newLocation;
            Debug.Log("Goto step "+ _currentVerticalStepIndex +" Position "+ _beforeDraggingVerticalPosition );
        }
        else{
            StartCoroutine(SmoothMove(draggingVerticalGameObject,draggingVerticalGameObject.transform.position, _beforeDraggingVerticalPosition, movingDuration));
            Debug.Log("Back to beginning Position"+ _beforeDraggingVerticalPosition );
        }
    }
    
    IEnumerator SmoothMove(GameObject movingObject, Vector3 startPos, Vector3 endPos, float seconds){
        float t = 0f;
        while(t <= 1.0){
            t += Time.deltaTime / seconds;
            movingObject.transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

}
