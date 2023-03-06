using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Calendar;
using UnityEngine;

public class CalendarListViewPage : MonoBehaviour
{
    /*
     * This is a class managing the Calendar List View Page
     * By displaying a certain amount of Time Block and adding, removing, changing them
     */
    
    [SerializeField] private GameObject content;
    
    [Header("TimeBlock List")]
    [SerializeField] private int numberOfRenderingTimeBlock = 30;
    [SerializeField] private TimeTable _displayingTimeTable;
    [SerializeField] private List<GameObject> _timeBlocks;


    private void OnEnable()
    {
        OnDisplay(DataManager.Instance.GetTimeTable());
    }
    
    
    public void OnDisplay(TimeTable timeTable)
    {
        _displayingTimeTable = timeTable;
        ClearAll();

        foreach (var subject in timeTable.subjectInfos)
        {
            CreateTimeBlock();
        }
    }

    private GameObject CreateTimeBlock()
    {
        GameObject instantiateTimeBlock = Instantiate(ResourceManager.Instance.timeBlockGo);

        return instantiateTimeBlock;
    }

    private void ClearAll()
    {
        _timeBlocks = new List<GameObject>();
    }
}
