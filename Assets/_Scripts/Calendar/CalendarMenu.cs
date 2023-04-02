using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CalendarMenu : SwipeVerticalHorizontalMenu
{
    // Start is called before the first frame update
    [Header("Calendar Menu Init")]
    [SerializeField] private RectTransform initVerticalPosition;

    protected override void OnEnable()
    {
        base.OnEnable();

        draggingVerticalGameObject.anchoredPosition = initVerticalPosition.anchoredPosition;
        SmoothMoveTo(draggingVerticalGameObject, expectDestinationVerticalPosition, movingDuration, verticalPages[initVerticalIndex].onSelectEvent);
    }

}
