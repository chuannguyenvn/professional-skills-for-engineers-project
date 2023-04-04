using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using _Scripts.Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeBlockSubject : TimeBlock
{
    private string room;
    [SerializeField] private TextMeshProUGUI firstHeader;
    [SerializeField] private TextMeshProUGUI secondHeader;
    [SerializeField] private Button navigationButton;
    
    public void Init(SubjectInfo subjectInfo, CalendarListViewPage calendarListViewPage, bool showDay = false)
    {
        firstHeader.text = subjectInfo.name;
        secondHeader.text = subjectInfo.lessonStartHour.Hours + ":" + subjectInfo.lessonStartHour.Minutes.ToString("D2") + " - " +
                            subjectInfo.lessonEndHour.Hours + ":" + subjectInfo.lessonEndHour.Minutes.ToString("D2") +
                            " tại " + subjectInfo.room;
        room = subjectInfo.room;
        _calendarListViewPage = calendarListViewPage;

        
    }

    public void Navigate()
    {
        string pattern = @"[a-c|A-C]\d+";
        Regex regex = new Regex(pattern);
        Match match = regex.Match(room);
        
        MapManager.Instance.Navigate(match.Value);
    }
    
}