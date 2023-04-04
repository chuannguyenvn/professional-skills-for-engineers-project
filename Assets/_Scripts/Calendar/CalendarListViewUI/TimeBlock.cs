using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TimeBlock: MonoBehaviour, IPointerDownHandler
{
    public DateTime dateTime;
    protected CalendarListViewPage _calendarListViewPage;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _calendarListViewPage.OnPointerDown(eventData);
    }
}
