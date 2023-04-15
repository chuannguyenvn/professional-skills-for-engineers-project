using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace _Scripts.Calendar
{
    public class DataManager : Singleton<DataManager>
    {
        private List<TimeTable> _timeTables = new();

        [Header("PlayerPrefs")] 
        private const string TIME_TABLE_COUNT_KEY = "Time Table Count";
        private const string TIME_TABLE_LIST_0_KEY = "Time Table Element ";

        protected override void Awake()
        {
            base.Awake();

            if (PlayerPrefs.HasKey(TIME_TABLE_COUNT_KEY))
            {
                
            }
            else
            {
                PlayerPrefs.SetInt(TIME_TABLE_COUNT_KEY, 0);
                PlayerPrefs.Save();
            }
        }

        public void AddTimeTable(TimeTable addingTimeTable)
        {
            _timeTables.Add(addingTimeTable);
            Debug.Log(addingTimeTable.originalText);
            PlayerPrefs.SetInt(TIME_TABLE_COUNT_KEY, _timeTables.Count);
            PlayerPrefs.SetString(TIME_TABLE_LIST_0_KEY + (_timeTables.Count-1).ToString(), addingTimeTable.originalText);
            PlayerPrefs.Save();
        }

        public TimeTable GetTimeTable(int index = 0)
        {
            if (index < 0 || index >= _timeTables.Count) return null;
            return _timeTables[index] ;
        }
        

        
        private TimeTable GetTimeTablePlayerPref(int index)
        {
            int count = PlayerPrefs.GetInt(TIME_TABLE_COUNT_KEY);
            if (index >= count || index < 0) return null;

            string originalText = PlayerPrefs.GetString(TIME_TABLE_LIST_0_KEY + index.ToString());
            return new TimeTable(originalText);
        }
        
        
        public void OnInputCheck(string input)
        {
            // Instantiate the regular expression object.
            Regex r = new Regex(
                @"Học kỳ (?<semester>\d) Năm học (?<yearFrom>\d+) - (?<yearTo>\d+)(\n|\r|\r|\n)[^(\n|\r|\r|\n)]*(\n|\r|\r|\n)[^(\n|\r|\r|\n)]*(\n|\r|\r|\n)(?<entries>(?:[^\a](?!Tổng số tín chỉ đăng ký))*)");

            // Match the regular expression pattern against a text string.
            _semesterMatchCollection = r.Matches(input);

            if (_semesterMatchCollection.Count > 0)
            {
                SetDropdownInfo();
            }
            else
            {
                Debug.Log("WHY NOT DETECT");
            }
        }

    }
}