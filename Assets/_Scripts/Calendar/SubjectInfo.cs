using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;


public class SubjectInfo
{
    public string subjectCode;
    public string name;
    public int credits;
    public string classGroup;
    public DayOfWeek date; 
    [Range(1, 20)]
    public TimeSpan lessonStartHour, lessonEndHour; 
    public string room;
    public List<DateTime> classDateTimes;


    public SubjectInfo(string nonSplitLineSubjectInfo)
    {
        Debug.Log(nonSplitLineSubjectInfo);
        string[] infoStrings = nonSplitLineSubjectInfo.Split("\t");
        //"MÃ MH	TÊN MÔN HỌC  	TÍN CHỈ  	TC HỌC PHÍ	  NHÓM-TỔ	 THỨ	TIẾT	GIỜ HỌC 	PHÒNG	CƠ SỞ	TUẦN HỌC"
        subjectCode = infoStrings[0];
        name = infoStrings[1];
        credits = int.Parse(infoStrings[2]);
        classGroup =  infoStrings[4];
        date = (DayOfWeek)(int.Parse(infoStrings[5]) %7 - 1 );
        (lessonStartHour, lessonEndHour)= StringToTimeSpan(infoStrings[7]);
        room = infoStrings[8];
        
    }

    private (TimeSpan, TimeSpan) StringToTimeSpan(string time)
    {
        return (TimeSpan.Zero, TimeSpan.Zero);
    }
    
    private void 
    
    public static DateTime FirstDateOfWeekIso8601(int year, int weekOfYear)
    {
        DateTime jan1 = new DateTime(year, 1, 1);
        int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

        // Use first Thursday in January to get first week of the year as
        // it will never be in Week 52/53
        DateTime firstThursday = jan1.AddDays(daysOffset);
        var cal = CultureInfo.CurrentCulture.Calendar;
        int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        var weekNum = weekOfYear;
        // As we're adding days to a date in Week 1,
        // we need to subtract 1 in order to get the right date for week #1
        if (firstWeek == 1)
        {
            weekNum -= 1;
        }

        // Using the first Thursday as starting week ensures that we are starting in the right year
        // then we add number of weeks multiplied with days
        var result = firstThursday.AddDays(weekNum * 7);

        // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
        return result.AddDays(-3);
    }
}