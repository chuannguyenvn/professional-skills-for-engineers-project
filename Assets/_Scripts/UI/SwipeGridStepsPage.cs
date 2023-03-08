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
    [SerializeField] private List<Transform> stepHorizontalTransforms;
    [SerializeField] private int _currentHorizontalStepIndex ;
    [SerializeField] private int _initHorizontalStepIndex = 0;
    [SerializeField] private float acceptHorizontalThreshHold = 0.2f;
    private Vector3 _beforeDraggingHorizontalPosition;

    
    
    [Header("Vertical Properties")]
    [SerializeField] private GameObject draggingVerticalGameObject;
    [SerializeField] private List<Transform> stepVerticalTransforms ;
    [SerializeField] private int _currentVerticalStepIndex ;
    [SerializeField] private int _initVerticalStepIndex=0 ;
    [SerializeField] private float acceptVerticalThreshHold = 0.2f;
    private Vector3 _beforeDraggingVerticalPosition;
    
    private void Start()
    {
        stepHorizontalTransforms = stepHorizontalTransforms.OrderBy(o => o.position.x).ToList();
        stepVerticalTransforms = stepVerticalTransforms.OrderBy(o => o.position.y).ToList();
        
        draggingHorizontalGameObject.transform.position = stepHorizontalTransforms[_initHorizontalStepIndex].position;
        draggingVerticalGameObject.transform.position = stepVerticalTransforms[_initVerticalStepIndex].position;
        
        StartCoroutine(SmoothMove(draggingHorizontalGameObject, draggingHorizontalGameObject.transform.position, stepHorizontalTransforms[_currentHorizontalStepIndex].position, movingDuration));
        StartCoroutine(SmoothMove(draggingVerticalGameObject, draggingVerticalGameObject.transform.position, stepVerticalTransforms[_currentVerticalStepIndex].position, movingDuration));

        _beforeDraggingHorizontalPosition = stepHorizontalTransforms[_currentHorizontalStepIndex].position;
        _beforeDraggingVerticalPosition = stepVerticalTransforms[_currentVerticalStepIndex].position;
    }
    
    
    
    public void OnDrag(PointerEventData eventData)
    {
        
        //Debug.Log("Drag "+ eventData.position + " - "+ eventData.pressPosition);

        float xDifference = eventData.pressPosition.x - eventData.position.x;
        float yDifference = eventData.pressPosition.y - eventData.position.y;
        if (_isFirstTimeDragging)
        {
            _isFirstTimeDragging = false;
            _isDraggingHorizontalNorVertical = xDifference >= yDifference;
        }
        else
        {
            if (_isDraggingHorizontalNorVertical)
            {
                transform.position = _beforeDraggingHorizontalPosition - new Vector3(xDifference, 0,0);
            }
            else
            {
                transform.position = _beforeDraggingVerticalPosition - new Vector3(0,yDifference ,0);
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
        float percentage = (eventData.pressPosition.x - eventData.position.x) / Screen.height;
        if(Mathf.Abs(percentage) >= acceptHorizontalThreshHold){
            Vector3 newLocation = _beforeDraggingHorizontalPosition;
            if(percentage < 0 && _currentHorizontalStepIndex < stepHorizontalTransforms.Count-1){
                _currentHorizontalStepIndex++;
                newLocation = stepHorizontalTransforms[_currentHorizontalStepIndex].position ;
            }
            else if(percentage > 0 && _currentHorizontalStepIndex >= 1){
                _currentHorizontalStepIndex--;
                newLocation = stepHorizontalTransforms[_currentHorizontalStepIndex].position ;
            }
        
            Debug.Log("Goto step "+ _currentHorizontalStepIndex +" Position "+ _beforeDraggingHorizontalPosition );
            StartCoroutine(SmoothMove(draggingHorizontalGameObject, draggingHorizontalGameObject.transform.position, newLocation, movingDuration));
            _beforeDraggingHorizontalPosition = newLocation;
        }
        else{
            Debug.Log("Back to beginning Position"+ _beforeDraggingHorizontalPosition );
            StartCoroutine(SmoothMove(draggingHorizontalGameObject,draggingHorizontalGameObject.transform.position, _beforeDraggingHorizontalPosition, movingDuration));
        }
    }
    
    
    private void OnVerticalEndDrag(PointerEventData eventData)
    {
        float percentage = (eventData.pressPosition.y - eventData.position.y) / Screen.width;
        if(Mathf.Abs(percentage) >= acceptVerticalThreshHold){
            Vector3 newLocation = _beforeDraggingVerticalPosition;
            if(percentage < 0 && _currentVerticalStepIndex < stepVerticalTransforms.Count-1){
                _currentVerticalStepIndex++;
                newLocation = stepVerticalTransforms[_currentVerticalStepIndex].position ;
            }
            else if(percentage > 0 && _currentVerticalStepIndex >= 1){
                _currentVerticalStepIndex--;
                newLocation = stepVerticalTransforms[_currentVerticalStepIndex].position ;
            }
        
            Debug.Log("Goto step "+ _currentVerticalStepIndex +" Position "+ _beforeDraggingVerticalPosition );
            StartCoroutine(SmoothMove(draggingVerticalGameObject, draggingVerticalGameObject.transform.position, newLocation, movingDuration));
            _beforeDraggingVerticalPosition = newLocation;
        }
        else{
            Debug.Log("Back to beginning Position"+ _beforeDraggingVerticalPosition );
            StartCoroutine(SmoothMove(draggingVerticalGameObject,draggingVerticalGameObject.transform.position, _beforeDraggingVerticalPosition, movingDuration));
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
