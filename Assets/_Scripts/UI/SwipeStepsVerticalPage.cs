using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeStepsVerticalPage : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("Properties")]
    [SerializeField] private float acceptThreshHold = 0.25f;
    [SerializeField] private float movingDuration = 0.5f;

    private Vector3 _panelLocation;
    [SerializeField] private List<Transform> stepTransforms ;
    private int _currentStepIndex;

    private void Start()
    {
        stepTransforms = stepTransforms.OrderBy(o => o.position.y).ToList();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag "+ eventData.position + " - "+ eventData.pressPosition);
        float difference = eventData.pressPosition.x - eventData.position.x;
        transform.position = _panelLocation - new Vector3(0, difference, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("EndDrag "+ eventData.position + " - "+ eventData.pressPosition);
        
        float percentage = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
        if(Mathf.Abs(percentage) >= acceptThreshHold){
            Vector3 newLocation = _panelLocation;
            if(percentage > 0 && _currentStepIndex < stepTransforms.Count){
                _currentStepIndex++;
                newLocation += stepTransforms[_currentStepIndex].position ;
            }else if(percentage < 0 && _currentStepIndex > 1){
                _currentStepIndex--;
                newLocation += stepTransforms[_currentStepIndex].position ;
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, movingDuration));
            _panelLocation = newLocation;
        }else{
            StartCoroutine(SmoothMove(transform.position, _panelLocation, movingDuration));
        }
    }
    IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float seconds){
        float t = 0f;
        while(t <= 1.0){
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}
