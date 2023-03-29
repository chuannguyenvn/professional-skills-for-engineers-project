using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TimeBlock<T> : MonoBehaviour, IPointerDownHandler
{
    protected CalendarListViewPage _calendarListViewPage;
    public abstract void Init(T info, CalendarListViewPage calendarListViewPage);
    public void OnPointerDown(PointerEventData eventData)
    {
        _calendarListViewPage.OnPointerDown(eventData);
    }
}
