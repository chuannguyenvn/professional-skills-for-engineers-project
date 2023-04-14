using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeBlockWeekGap : TimeBlock
{
    [SerializeField] private TextMeshProUGUI header1;
    [SerializeField] private Image background;
    public void Init(DateTime subjectInfo, CalendarListViewPage calendarListViewPage)
    {
        dateTime = subjectInfo;
        _calendarListViewPage = calendarListViewPage;

        DateTime nextWeek = subjectInfo.AddDays(7);
        header1.text = subjectInfo.ToString("MMM") + " " + subjectInfo.Day + " - " + nextWeek.ToString("MMM") + " " + nextWeek.Day ;
        background.sprite = VisualManager.Instance.GetRandomTimeBlockBackGround();
    }
}
