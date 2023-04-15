using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace _Scripts.Calendar
{
    public class DataManager : Singleton<DataManager>
    {
        private List<TimeTable> _timeTableCollection = new();

        [Header("PlayerPrefs")] 
        private const string TIME_TABLE_COUNT_KEY = "Time Table Count";
        private const string TIME_TABLE_LIST_0_KEY = "Time Table Element ";

        protected override void Awake()
        {
            base.Awake();

            if (PlayerPrefs.HasKey(TIME_TABLE_COUNT_KEY))
            {
                int count = PlayerPrefs.GetInt(TIME_TABLE_COUNT_KEY);
                Debug.Log("PlayerPref Timetable count "+ count );
                
                for (int i = 0; i < count; i++)
                {
                    TimeTable timeTable = GetTimeTablePlayerPref(i);

                    if (timeTable != null)
                    {
                        _timeTableCollection.Add(timeTable);
                    }
                    else
                    {
                        Debug.LogWarning("THERE WAS AN ERROR IN TIME TABLE GET PLAYERPREF " + i);
                    }
                }
            }
            else
            {
                PlayerPrefs.SetInt(TIME_TABLE_COUNT_KEY, 0);
                PlayerPrefs.Save();
            }
        }

        public void AddTimeTable(TimeTable addingTimeTable)
        {
            _timeTableCollection.Add(addingTimeTable);
            
            PlayerPrefs.SetInt(TIME_TABLE_COUNT_KEY, _timeTableCollection.Count);
            PlayerPrefs.SetString(TIME_TABLE_LIST_0_KEY + (_timeTableCollection.Count-1).ToString(), addingTimeTable.originalText);
            PlayerPrefs.Save();
        }

        public void ClearTimeTableCollection()
        {
            
        }

        public TimeTable GetTimeTable(int index = 0)
        {
            if (index < 0 || index >= _timeTableCollection.Count) return null;
            return _timeTableCollection[index] ;
        }

        public TimeTable GetLastTimeTable()
        {
            return _timeTableCollection[^1] ;
        }

        
        private TimeTable GetTimeTablePlayerPref(int index)
        {
            int count = PlayerPrefs.GetInt(TIME_TABLE_COUNT_KEY);
            if (index >= count || index < 0) return null;

            string originalText = PlayerPrefs.GetString(TIME_TABLE_LIST_0_KEY + index.ToString());
            
            Regex regex = new Regex(
                @"Học kỳ (?<semester>\d) Năm học (?<yearFrom>\d+) - (?<yearTo>\d+)(\n|\r|\r|\n)[^(\n|\r|\r|\n)]*(\n|\r|\r|\n)[^(\n|\r|\r|\n)]*(\n|\r|\r|\n)(?<entries>(?:[^\a](?!Tổng số tín chỉ đăng ký))*)");
            GroupCollection groups = regex.Match(originalText).Groups;
            TimeTable timeTable = new TimeTable(Int32.Parse(groups["semester"].Value),
                Int32.Parse(groups["yearFrom"].Value), groups["entries"].Value, groups[0].Value);
            
            return timeTable;
        }
        
        
    }
}