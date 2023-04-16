using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using _Scripts.Map;
using Unity.VisualScripting;
using UnityEngine;


public class SubjectInfo
{
    public string subjectCode;
    public string name;
    public int credits;
    public string classGroup;
    public DayOfWeek dayOfWeek;
    public TimeSpan lessonStartHour, lessonEndHour;
    public string room;
    public Building building;
    public List<DateTime> classDateTimes = new();
    public int startYear;

    public SubjectInfo(int startYear, string nonSplitLineSubjectInfo)
    {
        //Debug.Log(nonSplitLineSubjectInfo);

        this.startYear = startYear;

        string[] infoStrings = nonSplitLineSubjectInfo.Split("\t");
        //"MÃ MH	TÊN MÔN HỌC  	TÍN CHỈ  	TC HỌC PHÍ	  NHÓM-TỔ	 THỨ	TIẾT	GIỜ HỌC 	PHÒNG	CƠ SỞ	TUẦN HỌC"

        subjectCode = infoStrings[0];
        name = infoStrings[1];


        credits = infoStrings[2] != "--" ? int.Parse(infoStrings[2]) : 0;
        
        classGroup = infoStrings[4];
        dayOfWeek = infoStrings[5] != "--" ? (DayOfWeek)(int.Parse(infoStrings[5]) % 7 - 1): (DayOfWeek) 1;
        DecodeIntoTimeSpan(infoStrings[7]);

        string buildingPattern = @"[a-c|A-C]\d+", roomPattern = @"[\d]{3}";
        Regex regex1 = new Regex(buildingPattern), regex2 = new Regex(roomPattern);
        Match match1 = regex1.Match(infoStrings[8]), match2 = regex2.Match(infoStrings[8]);
        
        building = MapManager.Instance.FindBuilding(match1.Value);
        room = match2.Value;
        
        DecodeWeekString(infoStrings[10]);
    }

    private void DecodeIntoTimeSpan(string time)
    {
        Regex pattern = new Regex(@"(\d+):(\d+) - (\d+):(\d+)");
        Match match = pattern.Match(time);

        int startHour = Int32.Parse(match.Groups[1].Value);
        int startMinute = Int32.Parse(match.Groups[2].Value);

        int endHour = Int32.Parse(match.Groups[3].Value);
        int endMinute = Int32.Parse(match.Groups[4].Value);

        lessonStartHour = new TimeSpan(startHour, startMinute, 0);
        lessonEndHour = new TimeSpan(endHour, endMinute, 0);
    }

    private void DecodeWeekString(string weeksLine)
    {
        string[] weeks = weeksLine.Split("|");

        foreach (var week in weeks)
        {
            if (week == "--" || week.Length != 2) continue;
            
            DateTime dateTime = CreateClassDateTimes(int.Parse(week));
            classDateTimes.Add(dateTime);
        }
    }


    private DateTime CreateClassDateTimes(int week)
    {
        DateTime dateTime = FirstDateOfWeekIso8601(startYear, week);
        dateTime = dateTime.AddDays(dayOfWeek - DayOfWeek.Monday);
        dateTime = dateTime.Add(lessonStartHour);

        //Debug.Log(name + " date " + dateTime.ToString() + " dayOfWeek " + dayOfWeek + " start at " +lessonStartHour.ToString() + " end at " + lessonEndHour.ToString());

        return dateTime;
    }

    
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
    
    public static int WeekOfYearIso8601(DateTime date)
    {
        var cal = CultureInfo.CurrentCulture.Calendar;
        int weekOfYear = cal.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        // Handle the special case where the date falls in the last week of the previous year
        if (weekOfYear >= 52 && date.Month == 1)
        {
            weekOfYear = 0;
        }

        return weekOfYear;
    }
}