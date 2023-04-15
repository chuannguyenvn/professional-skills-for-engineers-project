using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using Match = System.Text.RegularExpressions.Match;


namespace _Scripts.Calendar
{
    public class CalendarCreationPage : HorizontalSwipePageBase
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI comment;
        [SerializeField] private TMP_Dropdown semesterDropdown;

        private MatchCollection _semesterMatchCollection;
        
        
        private void OnEnable()
        {
            
        }

        void Start()
        {
            
        }

        public void OnFastPaste()
        {
            string clipBoard = GUIUtility.systemCopyBuffer;
            inputField.text = clipBoard;
            OnInputCheck(inputField.text);
        }

        public void OnClearAll()
        {
            inputField.text = String.Empty;
            OnInputCheck(inputField.text);
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

        public void OnSubmission()
        {
            int selectedSemester = semesterDropdown.value;
            GroupCollection groups = _semesterMatchCollection[selectedSemester].Groups;
            TimeTable timeTable = new TimeTable(Int32.Parse(groups["semester"].Value),
                Int32.Parse(groups["yearFrom"].Value), groups["entries"].Value, groups[0].Value);
            DataManager.Instance.AddTimeTable(timeTable);
        }

        private void SetDropdownInfo()
        {
            List<TMP_Dropdown.OptionData> semesterDropdownOptions = new List<TMP_Dropdown.OptionData>();
            Debug.Log("Found "+ _semesterMatchCollection.Count+ " Semesters");
            foreach (Match match in _semesterMatchCollection)
            {
                TMP_Dropdown.OptionData semesterData = new TMP_Dropdown.OptionData();
                string yearFrom = match.Groups["yearFrom"].Value.Substring(match.Groups["yearFrom"].Value.Length - 2);
                string semester = match.Groups["semester"].Value;
                semesterData.text = "Học Kỳ " + yearFrom + semester;
                semesterDropdownOptions.Add(semesterData);

                semesterDropdown.options = semesterDropdownOptions;
            }
        }

        
    }
}