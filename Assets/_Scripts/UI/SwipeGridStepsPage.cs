using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
public class SwipeGridStepsPage : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("Properties")]
    [SerializeField] private float acceptThreshHold = 0.25f;
    [SerializeField] private float movingDuration = 0.5f;

    [Header("Horizontal Properties")] 
    [SerializeField] private GameObject movingHorizontalGameObject; 
    [SerializeField] private List<Transform> stepHorizontalTransforms;
    [SerializeField] private int _currentHorizontalStepIndex ;
    [SerializeField] private int _initHorizontalStepIndex = 0;

    [Header("Vertical Properties")] private GameObject movingVerticalGameObject;
    [SerializeField] private List<Transform> stepVerticalTransforms ;
    [SerializeField] private int _currentVerticalStepIndex ;
    [SerializeField] private int _initVerticalStepIndex=0 ;
    
    private Vector3 _panelLocation;

    private void Start()
    {
        stepHorizontalTransforms = stepHorizontalTransforms.OrderBy(o => o.position.x).ToList();
        stepHorizontalTransforms = stepVerticalTransforms.OrderBy(o => o.position.y).ToList();
        
        movingHorizontalGameObject.transform.position = stepHorizontalTransforms[_initHorizontalStepIndex].position;
        movingVerticalGameObject.transform.position = stepVerticalTransforms[_initVerticalStepIndex].position;
        
        StartCoroutine(SmoothMove(movingHorizontalGameObject, movingHorizontalGameObject.transform.position, stepHorizontalTransforms[_currentHorizontalStepIndex].position, movingDuration));
        StartCoroutine(SmoothMove(movingVerticalGameObject, movingVerticalGameObject.transform.position, stepVerticalTransforms[_currentVerticalStepIndex].position, movingDuration));

        _panelLocation = stepHorizontalTransforms[_currentHorizontalStepIndex].position;
        _panelLocation = stepHorizontalTransforms[_currentHorizontalStepIndex].position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag "+ eventData.position + " - "+ eventData.pressPosition);
        float difference = eventData.pressPosition.x - eventData.position.x;
        transform.position = _panelLocation - new Vector3(difference, 0,0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("EndDrag "+ eventData.position + " - "+ eventData.pressPosition);
        float percentage = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
        if(Mathf.Abs(percentage) >= acceptThreshHold){
            Vector3 newLocation = _panelLocation;
            if(percentage < 0 && _currentHorizontalStepIndex < stepHorizontalTransforms.Count-1){
                _currentHorizontalStepIndex++;
                newLocation = stepHorizontalTransforms[_currentHorizontalStepIndex].position ;
            }else if(percentage > 0 && _currentHorizontalStepIndex >= 1){
                _currentHorizontalStepIndex--;
                newLocation = stepHorizontalTransforms[_currentHorizontalStepIndex].position ;
            }
        
            Debug.Log("Goto step "+ _currentHorizontalStepIndex +" Position "+ _panelLocation );
            StartCoroutine(SmoothMove(transform.position, newLocation, movingDuration));
            _panelLocation = newLocation;
        }else{
            Debug.Log("Back to beginning Position"+ _panelLocation );
            StartCoroutine(SmoothMove(transform.position, _panelLocation, movingDuration));
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
