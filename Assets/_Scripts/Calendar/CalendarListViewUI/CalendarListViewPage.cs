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
    
    [Header("Data structure")] 
    [SerializeField] private List<TimeBlock> timeBlocks = new();
    
    private Dictionary<DateTime,SubjectInfo> _dateTimeAndSubjectInfosDictionary = new();

    
    public void OnEnterPage()
    {
        ClearAllTimeBlock();
        GetSubjectInfo(DataManager.Instance.GetTimeTable());
        
        DisplayManyMedianWeeks(DateTime.Now, 10, 10);
        content.pivot = new Vector2(0.5f,0.5f);

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

        Debug.Log("Change " + amount + " index "+ topItemIndex);
        return;
        if ( (float)(currentTopIndex - topItemIndex)/timeBlocks.Count <= topLoadPercentage )
        {
            Debug.Log("Load more at the top"+ currentTopIndex+ " " + topItemIndex + " " + timeBlocks.Count);
            var firstTimeBlock = timeBlocks[0];
            DisplaySubjectInRange(firstTimeBlock.dateTime + new TimeSpan(-7,0,0,0), firstTimeBlock.dateTime, false, true);
            CreateTimeBlockDayGap(firstTimeBlock.dateTime + new TimeSpan(-7,0,0,0), true);
            //firstIndex = Mathf.RoundToInt(oldCount  * (timeBlocks.Count));
        }

        if ( (float)(topItemIndex - currentBottomIndex)/timeBlocks.Count <= bottomLoadPercentage)
        {
            Debug.Log("Load more at the bottom "+ currentBottomIndex+" " + topItemIndex + " " + timeBlocks.Count);
            var firstTimeBlock = timeBlocks[^1];
            DisplaySubjectInRange(firstTimeBlock.dateTime + new TimeSpan(7,0,0,0), firstTimeBlock.dateTime);
            CreateTimeBlockDayGap(firstTimeBlock.dateTime + new TimeSpan(7,0,0,0));
        }
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
            CreateTimeBlockDayGap(endOfPreviousWeek);

            startOfPreviousWeek = startOfPreviousWeek.AddDays(+7);
            endOfPreviousWeek = endOfPreviousWeek.AddDays(+7);
        }
        
        DisplaySubjectInRange(startOfWeek, endOfWeek);
        CreateTimeBlockDayGap(startOfWeek);
        
        for (int i = 0 ; i< numberOfFollowingWeek; i++)
        {
            DisplaySubjectInRange(startOfFollowingWeek, endOfFollowingWeek);
            CreateTimeBlockDayGap(endOfFollowingWeek);

            startOfFollowingWeek = startOfFollowingWeek.AddDays(7);
            endOfFollowingWeek = endOfFollowingWeek.AddDays(7);
        }

    }

    private void DisplaySubjectInRange(DateTime startTime, DateTime endTime, bool showDay = false, bool isTopNorBottom = false)
    {
        List<KeyValuePair<DateTime, SubjectInfo>> valuesInRange = _dateTimeAndSubjectInfosDictionary
            .Where(kv => kv.Key >= startTime && kv.Key <= endTime)
            .Select(kv => kv )
            .ToList();
        foreach (var keyValuePair in valuesInRange)
        {
            CreateTimeBlockSubject(keyValuePair.Key, keyValuePair.Value, showDay, isTopNorBottom);
        }
    }

    #region Creation

    private void CreateTimeBlockSubject(DateTime dateTime, SubjectInfo subject, bool showDay = false, bool isTopNorBottom = false)
    {
        var instantiateTimeBlock = Instantiate(ResourceManager.Instance.timeBlockSubject, content.transform);
        instantiateTimeBlock.GetComponent<TimeBlockSubject>().Init(dateTime, subject, this);
        Debug.Log("create timeblock "+ subject.name +" At " +dateTime.ToString());
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

    private void CreateTimeBlockDayGap(DateTime dateTime,bool isTopNorBottom = false)
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