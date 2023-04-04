using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeBlockDayGap : TimeBlock
{
    [SerializeField] private TextMeshProUGUI header1;
    public void Init(DateTime subjectInfo, CalendarListViewPage calendarListViewPage)
    {
        dateTime = subjectInfo;
        _calendarListViewPage = calendarListViewPage;

        header1.text = subjectInfo.ToString();
    }
}
