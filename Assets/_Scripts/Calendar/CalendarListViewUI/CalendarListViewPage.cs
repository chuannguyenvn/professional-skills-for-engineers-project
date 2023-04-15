using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Calendar;
using _Scripts.Manager;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// This is a class managing the Calendar List View Page
///  By displaying a certain amount of Time Block and adding, removing, changing them
/// </summary>
public class CalendarListViewPage : HorizontalSwipePageBase
{
    [SerializeField] private RectTransform content;
    
    [Header("TimeBlock List")] 
    [SerializeField] private int numberOfRenderingTimeBlock = 30;

    [SerializeField, Range(0,0.5f)] private float bottomLoadPercentage = 0.2f;
    [SerializeField, Range(0.5f,1)] private float topLoadPercentage = 0.8f;
    private int currentTopIndex, currentBottomIndex;
    private TimeTable _displayingTimeTable;
    
    [Header("Time Table Properties")] 
    [SerializeField] private List<TimeBlock> timeBlocks = new();
    private DateTime _firstClassDate, _lastClassDate;
    private Dictionary<DateTime,SubjectInfo> _dateTimeAndSubjectInfosDictionary = new();

    
    public void OnEnterPage()
    {
        ClearAllTimeBlock();
        GetSubjectInfo(DataManager.Instance.GetLastTimeTable());
        
        if(_displayingTimeTable == null) DisplayManyMedianWeeks(DateTime.Now, 10, 10);
        else DisplaySubjectInRange(_firstClassDate, _lastClassDate, true);
        
        currentTopIndex = timeBlocks.Count;
        currentBottomIndex = 0;
    }

    public void OnOutPage()
    {
        ClearAllTimeBlock();
    }

    public void OnScrollValueChange(Vector2 amount)
    {
        int topItemIndex = Mathf.RoundToInt(amount.y * (timeBlocks.Count));

        return;
        Debug.Log("Change " + amount + " index "+ topItemIndex);

        if ( (float)(currentTopIndex - topItemIndex)/timeBlocks.Count <= topLoadPercentage )
        {
            Debug.Log("Load more at the top"+ currentTopIndex+ " " + topItemIndex + " " + timeBlocks.Count);
            var firstTimeBlock = timeBlocks[0];
            DisplaySubjectInRange(firstTimeBlock.dateTime + new TimeSpan(-7,0,0,0), firstTimeBlock.dateTime, false, true);
            CreateTimeBlockWeekGap(firstTimeBlock.dateTime + new TimeSpan(-7,0,0,0), true);
            //firstIndex = Mathf.RoundToInt(oldCount  * (timeBlocks.Count));
        }

        if ( (float)(topItemIndex - currentBottomIndex)/timeBlocks.Count <= bottomLoadPercentage)
        {
            Debug.Log("Load more at the bottom "+ currentBottomIndex+" " + topItemIndex + " " + timeBlocks.Count);
            var firstTimeBlock = timeBlocks[^1];
            DisplaySubjectInRange(firstTimeBlock.dateTime + new TimeSpan(7,0,0,0), firstTimeBlock.dateTime);
            CreateTimeBlockWeekGap(firstTimeBlock.dateTime + new TimeSpan(7,0,0,0));
        }
    }

    private void GetSubjectInfo(TimeTable timeTable)
    {
        if(timeTable == null) return;
        
        _displayingTimeTable = timeTable;
        _dateTimeAndSubjectInfosDictionary = new();
        DateTime firstDateTime = DateTime.MaxValue;
        DateTime lastDateTime = DateTime.MinValue;

        foreach (var subjectInfo in timeTable.subjectInfos)
        {
            foreach (var dateTime in subjectInfo.classDateTimes)
            {
                if(!_dateTimeAndSubjectInfosDictionary.ContainsKey(dateTime)) _dateTimeAndSubjectInfosDictionary.Add(dateTime, subjectInfo);
                
                if (dateTime < firstDateTime)
                {
                    firstDateTime = dateTime;
                }

                if (dateTime > lastDateTime)
                {
                    lastDateTime = dateTime;
                }
            }
        }

        _firstClassDate = firstDateTime;
        _lastClassDate = lastDateTime;
    }

    private void DisplayManyMedianWeeks(DateTime baseDateTime, int numberOfPreviousWeeks = 1, int numberOfFollowingWeek = 1 )
    {
        // Calculate the start and end dates of the current week
        DateTime startOfWeek = baseDateTime.Date.AddDays(-(int)baseDateTime.DayOfWeek +1); //Start week is monday
        DateTime endOfWeek = startOfWeek.AddDays(6);

        // Calculate the start and end dates of the previous week
        DateTime startOfPreviousWeek = startOfWeek.AddDays(-7* numberOfPreviousWeeks);
        DateTime endOfPreviousWeek = endOfWeek.AddDays(-7 * numberOfPreviousWeeks);

        // Calculate the start and end dates of the following week
        DateTime startOfFollowingWeek = startOfWeek.AddDays(7);
        DateTime endOfFollowingWeek = endOfWeek.AddDays(7);
        
        
        for (int i = 0 ; i< numberOfPreviousWeeks; i++)
        {
            DisplaySubjectInRange(startOfPreviousWeek, endOfPreviousWeek);
            CreateTimeBlockWeekGap(endOfPreviousWeek);

            startOfPreviousWeek = startOfPreviousWeek.AddDays(+7);
            endOfPreviousWeek = endOfPreviousWeek.AddDays(+7);
        }
        
        DisplaySubjectInRange(startOfWeek, endOfWeek);
        CreateTimeBlockWeekGap(startOfWeek);
        
        for (int i = 0 ; i< numberOfFollowingWeek; i++)
        {
            DisplaySubjectInRange(startOfFollowingWeek, endOfFollowingWeek);
            CreateTimeBlockWeekGap(endOfFollowingWeek);

            startOfFollowingWeek = startOfFollowingWeek.AddDays(7);
            endOfFollowingWeek = endOfFollowingWeek.AddDays(7);
        }

    }

    private void DisplaySubjectInRange(DateTime startTime, DateTime endTime, bool showWeekGap = false, bool isTopNorBottom = false)
    {
        List<KeyValuePair<DateTime, SubjectInfo>> valuesInRange = _dateTimeAndSubjectInfosDictionary
            .Where(kv => kv.Key >= startTime && kv.Key <= endTime)
            .OrderBy(kv => kv.Key)
            .ToList();
        // Calculate the start and end dates of the current week
        DateTime startOfWeek = startTime.Date.AddDays(-(int)startTime.DayOfWeek +1); //Start week is monday
        
        if (showWeekGap)
        {
            int index = 0;
            while (startOfWeek < endTime && index < valuesInRange.Count)
            {
                DateTime nextWeek = startOfWeek.AddDays(7);
                CreateTimeBlockWeekGap(startOfWeek);
                while (index < valuesInRange.Count && valuesInRange[index].Key < nextWeek)
                {
                    if (index > 0)
                    {
                        CreateTimeBlockSubject(valuesInRange[index].Key, valuesInRange[index].Value, ShowDay(valuesInRange[index].Key, valuesInRange[index-1].Key));
                    }
                    else CreateTimeBlockSubject(valuesInRange[index].Key, valuesInRange[index].Value, true);
                    
                    index++;
                }
                startOfWeek = nextWeek;
            }

        }
        else
        {
            foreach (var keyValuePair in valuesInRange)
            {
                CreateTimeBlockSubject(keyValuePair.Key, keyValuePair.Value, false, isTopNorBottom);
            }    
        } 
        
    }

    private bool ShowDay(DateTime current, DateTime before)
    {
        return !(current.Day == before.Day && current.Month == before.Month && current.Year == before.Year);
    }

    #region Creation

    private void CreateTimeBlockSubject(DateTime dateTime, SubjectInfo subject, bool showDay = false, bool isTopNorBottom = false)
    {
        TimeBlockSubject instantiateTimeBlock;
        if (dateTime >= DateTime.Today) instantiateTimeBlock = Instantiate(ResourceManager.Instance.timeBlockSubject, content.transform);
        else instantiateTimeBlock = Instantiate(ResourceManager.Instance.timeBlockOldSubject, content.transform);
        instantiateTimeBlock.GetComponent<TimeBlockSubject>().Init(dateTime, subject, this, showDay);
        //Debug.Log("create timeblock "+ subject.name +" At " +dateTime.ToString());
        if (isTopNorBottom)
        {
            instantiateTimeBlock.transform.SetAsFirstSibling();
            timeBlocks.Insert(0, instantiateTimeBlock);
            currentTopIndex++;
            if(timeBlocks.Count > numberOfRenderingTimeBlock) DestroyTimeBlock(false);
        }
        else
        {
            instantiateTimeBlock.transform.SetAsLastSibling();
            timeBlocks.Add(instantiateTimeBlock);
            currentBottomIndex--;
            if(timeBlocks.Count > numberOfRenderingTimeBlock) DestroyTimeBlock(true);
        }
        
    }
    

    private void CreateTimeBlockWeekGap(DateTime dateTime,bool isTopNorBottom = false)
    {
        var instantiateTimeBlock = Instantiate(ResourceManager.Instance.timeBlockWeekGap, content.transform);
        instantiateTimeBlock.GetComponent<TimeBlockWeekGap>().Init(dateTime, this);
        if (isTopNorBottom)
        {
            instantiateTimeBlock.transform.SetAsFirstSibling();
            timeBlocks.Insert(0, instantiateTimeBlock);
            currentTopIndex++;
            if(timeBlocks.Count > numberOfRenderingTimeBlock) DestroyTimeBlock(false);
        }
        else
        {
            instantiateTimeBlock.transform.SetAsLastSibling();
            timeBlocks.Add(instantiateTimeBlock);
            currentBottomIndex--;
            if(timeBlocks.Count > numberOfRenderingTimeBlock) DestroyTimeBlock(true);
        }
        
    }
    #endregion

    private void DestroyTimeBlock(bool isTopNorBottom)
    {
        return;
        if (isTopNorBottom)
        {
            Destroy(timeBlocks[0].gameObject);
            timeBlocks.RemoveAt(0);
        }
        else
        {
            int last = timeBlocks.Count - 1;
            Destroy(timeBlocks[last].gameObject);
            timeBlocks.RemoveAt(last);
        }
        
    }
    
    private void ClearAllTimeBlock()
    {
        foreach (var timeBlock in timeBlocks)
        {
            Destroy(timeBlock.gameObject);
        }

        timeBlocks = new();
    }
}