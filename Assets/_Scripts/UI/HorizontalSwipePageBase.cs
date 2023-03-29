using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HorizontalSwipePageBase : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Base")]
    [SerializeField] private SwipeVerticalHorizontalMenu menu;
    [SerializeField] private ScrollRect scrollRect;
    
    private bool _isFirstTimeDragging = true;
    bool _isDraggingHorizontalNorVertical = false;

    private void Start()
    {
        if (menu == null)
        {
            Debug.LogError("No menu in page");
        }

        scrollRect = gameObject.GetComponent<ScrollRect>();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Child pointer down");
        menu.OnPointerDownHorizontalChildPage(eventData);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        float xDifference = eventData.position.x - eventData.pressPosition.x;
        float yDifference = eventData.position.y - eventData.pressPosition.y;
        
        _isDraggingHorizontalNorVertical = Mathf.Abs( xDifference) >= Mathf.Abs( yDifference);
        if (_isDraggingHorizontalNorVertical)
        {
            Debug.Log("child begin horizontal drag");
            menu.OnBeginDragHorizontalChildPage(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDraggingHorizontalNorVertical)
        {
            menu.OnDragHorizontalChildPage(eventData);
            scrollRect.vertical = false;
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isDraggingHorizontalNorVertical)
        {
            menu.OnEndDragHorizontalChildPage(eventData);
            scrollRect.vertical = true;
            _isFirstTimeDragging = true;
            _isDraggingHorizontalNorVertical = false;
        }
        else
        {
            _isFirstTimeDragging = true;
        }
    }

}
