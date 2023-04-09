using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using _Scripts.Manager;
using _Scripts.Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeBlockSubject : TimeBlock
{
    public SubjectInfo subjectInfo;
    private Building building;
    [SerializeField] private TextMeshProUGUI firstHeader;
    [SerializeField] private TextMeshProUGUI secondHeader;
    [SerializeField] private Button navigationButton;
    [SerializeField] private TextMeshProUGUI dayOfWeek;
    [SerializeField] private TextMeshProUGUI date;
    public void Init(DateTime dateTime, SubjectInfo subjectInfo, CalendarListViewPage calendarListViewPage, bool showDay = false)
    {
        this.dateTime = dateTime;
        this.subjectInfo = subjectInfo;
        _calendarListViewPage = calendarListViewPage;
        building = subjectInfo.building;

        
        firstHeader.text = subjectInfo.name;
        if (building != null)
        {
            secondHeader.text = subjectInfo.lessonStartHour.Hours + ":" + subjectInfo.lessonStartHour.Minutes.ToString("D2") + " - " +
                                subjectInfo.lessonEndHour.Hours + ":" + subjectInfo.lessonEndHour.Minutes.ToString("D2") +
                                " tại "+ subjectInfo.building.buildingSo.buildingName +"-"+ subjectInfo.room;
        }
        else
        {
            secondHeader.text = subjectInfo.lessonStartHour.Hours + ":" + subjectInfo.lessonStartHour.Minutes.ToString("D2") + " - " +
                                subjectInfo.lessonEndHour.Hours + ":" + subjectInfo.lessonEndHour.Minutes.ToString("D2") +
                                " tại nhà";
        }
        
        dayOfWeek.text = dateTime.DayOfWeek.ToString().Substring(0,3);
        date.text = dateTime.Day.ToString();
        
        navigationButton.onClick.AddListener(() => ApplicationManager.Instance.SetState(AppState.Navigate, null, new object[]{building} ));
    }

    
    
}