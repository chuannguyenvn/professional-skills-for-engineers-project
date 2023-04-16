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
    public void Init(DateTime dateTime, CalendarListViewPage calendarListViewPage)
    {
        base.dateTime = dateTime;
        _calendarListViewPage = calendarListViewPage;

        DateTime nextWeek = dateTime.AddDays(7);
        //header1.text = subjectInfo.ToString("MMM") + " " + subjectInfo.Day + " - " + nextWeek.ToString("MMM") + " " + nextWeek.Day ;
        int week = SubjectInfo.WeekOfYearIso8601(dateTime);
        header1.text = "WEEK " + week; 
        background.sprite = VisualManager.Instance.GetRandomTimeBlockBackGround();
    }
}
