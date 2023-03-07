using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Calendar;
using _Scripts.Manager;
using UnityEngine;

/// <summary>
/// This is a class managing the Calendar List View Page
///  By displaying a certain amount of Time Block and adding, removing, changing them
/// </summary>
public class CalendarListViewPage : MonoBehaviour
{
    [SerializeField] private GameObject content;

    [Header("TimeBlock List")] 
    [SerializeField] private int numberOfRenderingTimeBlock = 30;
    [SerializeField] private TimeTable _displayingTimeTable;

    [Header("Data structure")] 
    [SerializeField] private List<GameObject> _timeBlocks = new();
    
    private Dictionary<DateTime,SubjectInfo> _dateTimeAndSubjectInfosDictionary = new();

    private void OnEnable()
    {
        GetSubjectInfo(DataManager.Instance.GetTimeTable());
        ClearAllTimeBlock();
        
        DisplayManyMedianWeeks(DateTime.Now, 10, 10);
    }

    private void GetSubjectInfo(TimeTable timeTable)
    {
        if(timeTable == null) return;
        
        _displayingTimeTable = timeTable;
        foreach (var subjectInfo in timeTable.subjectInfos)
        {
            foreach (var dateTime in subjectInfo.classDateTimes)
            {
                _dateTimeAndSubjectInfosDictionary.Add(dateTime, subjectInfo);
            }
        }
    }

    private void DisplayManyMedianWeeks(DateTime baseDateTime, int numberOfPreviousWeeks = 1, int numberOfFollowingWeek = 1 )
    {
        // Calculate the start and end dates of the current week
        DateTime startOfWeek = baseDateTime.Date.AddDays(-(int)baseDateTime.DayOfWeek);
        DateTime endOfWeek = startOfWeek.AddDays(6);

        // Calculate the start and end dates of the previous week
        DateTime startOfPreviousWeek = startOfWeek.AddDays(-7);
        DateTime endOfPreviousWeek = endOfWeek.AddDays(-7);

        // Calculate the start and end dates of the following week
        DateTime startOfFollowingWeek = startOfWeek.AddDays(7);
        DateTime endOfFollowingWeek = endOfWeek.AddDays(7);
        
        CreateTimeBlockDayGap(startOfFollowingWeek);
        Display1Week(startOfFollowingWeek, endOfFollowingWeek);

        for (int i = 0 ; i< numberOfPreviousWeeks; i++)
        {
            Display1Week(startOfPreviousWeek, endOfPreviousWeek);
            CreateTimeBlockDayGap(endOfPreviousWeek);

            startOfPreviousWeek = startOfPreviousWeek.AddDays(-7);
            endOfPreviousWeek = endOfPreviousWeek.AddDays(-7);
        }
        
        Display1Week(startOfWeek, endOfWeek);
        CreateTimeBlockDayGap(startOfWeek);
        
        for (int i = 0 ; i< numberOfFollowingWeek; i++)
        {
            Display1Week(startOfFollowingWeek, endOfFollowingWeek);
            CreateTimeBlockDayGap(endOfFollowingWeek);

            startOfFollowingWeek = startOfFollowingWeek.AddDays(-7);
            endOfFollowingWeek = endOfFollowingWeek.AddDays(-7);
        }

    }

    private void Display1Week(DateTime startTime, DateTime endTime)
    {
        List<SubjectInfo> valuesInRange = _dateTimeAndSubjectInfosDictionary
            .Where(kv => kv.Key >= startTime && kv.Key <= endTime)
            .Select(kv => kv.Value)
            .ToList();
        foreach (var subjectInfo in valuesInRange)
        {
            CreateTimeBlockSubject(subjectInfo);
        }
    }

    #region Creation

    private void CreateTimeBlockSubject(SubjectInfo subject)
    {
        GameObject instantiateTimeBlock = Instantiate(ResourceManager.Instance.timeBlockSubjectGo, content.transform);
        instantiateTimeBlock.GetComponent<TimeBlockSubject>().Init(subject);
    }

    private void CreateTimeBlockDayGap(DateTime dateTime)
    {
        GameObject instantiateTimeBlock = Instantiate(ResourceManager.Instance.timeBlockDayGapGo, content.transform);
        instantiateTimeBlock.GetComponent<TimeBlockDayGap>().Init(dateTime);
    }
    #endregion
   
    private void ClearAllTimeBlock()
    {
        _timeBlocks = new List<GameObject>();
    }
}