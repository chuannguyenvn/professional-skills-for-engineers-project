using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeBlockDayGap : TimeBlock<DateTime>
{
    [SerializeField] private TextMeshProUGUI header1;
    public override void Init(DateTime subjectInfo, CalendarListViewPage calendarListViewPage)
    {
        header1.text = subjectInfo.ToString();
        _calendarListViewPage = calendarListViewPage;
    }
}
