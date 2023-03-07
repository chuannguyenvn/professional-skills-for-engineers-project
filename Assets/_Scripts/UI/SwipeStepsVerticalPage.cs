using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeStepsVerticalPage : MonoBehaviour, IDragHandler, IEndDragHandler
{

    [SerializeField] private float middlePage = 0.25f;
    
    [SerializeField] private float acceptThreshHold = 0.25f;
    
    void Start()
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag "+ eventData.position + " - "+ eventData.pressPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag "+ eventData.position + " - "+ eventData.pressPosition);
    }
}
