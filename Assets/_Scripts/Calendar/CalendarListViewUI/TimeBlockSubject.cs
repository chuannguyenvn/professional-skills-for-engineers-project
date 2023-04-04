using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using _Scripts.Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeBlockSubject : TimeBlock
{
    public SubjectInfo subjectInfo;
    private string room;
    [SerializeField] private TextMeshProUGUI firstHeader;
    [SerializeField] private TextMeshProUGUI secondHeader;
    [SerializeField] private Button navigationButton;
    
    public void Init(DateTime dateTime, SubjectInfo subjectInfo, CalendarListViewPage calendarListViewPage, bool showDay = false)
    {
        this.dateTime = dateTime;
        this.subjectInfo = subjectInfo;
        _calendarListViewPage = calendarListViewPage;
        
        
        firstHeader.text = subjectInfo.name;
        secondHeader.text = subjectInfo.lessonStartHour.Hours + ":" + subjectInfo.lessonStartHour.Minutes.ToString("D2") + " - " +
                            subjectInfo.lessonEndHour.Hours + ":" + subjectInfo.lessonEndHour.Minutes.ToString("D2") +
                            " tại " + subjectInfo.room;
        room = subjectInfo.room;
    }

    public void Navigate()
    {
        string pattern = @"[a-c|A-C]\d+";
        Regex regex = new Regex(pattern);
        Match match = regex.Match(room);
        
        MapManager.Instance.Navigate(match.Value);
    }
    
}