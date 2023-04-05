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

        header1.text = subjectInfo.ToString();
        background.sprite = VisualManager.Instance.GetRandomTimeBlockBackGround();
    }
}
