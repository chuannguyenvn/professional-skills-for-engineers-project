using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HorizontalSwipePageBase : MonoBehaviour
{
    [SerializeField] private SwipeVerticalHorizontalMenu menu;
    [SerializeField] private ScrollRect scrollRect;
    
    private bool _isFirstTimeDragging = true;
    private void Start()
    {
        if (menu == null)
        {
            Debug.LogError("No menu in page");
        }

        scrollRect = GetComponent<ScrollRect>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        float xDifference = eventData.position.x - eventData.pressPosition.x;
        float yDifference = eventData.position.y - eventData.pressPosition.y;
        bool isDraggingHorizontalNorVertical = false;
        
        if (_isFirstTimeDragging)
        {
            _isFirstTimeDragging = false;
            isDraggingHorizontalNorVertical = Mathf.Abs( xDifference) >= Mathf.Abs( yDifference);
        }

        if (isDraggingHorizontalNorVertical)
        {
            menu.OnDragHorizontalChildPage(eventData);
            scrollRect.vertical = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        menu.OnEndDragHorizontalChildPage(eventData);
        scrollRect.vertical = true;
        _isFirstTimeDragging = true;
    }
}
