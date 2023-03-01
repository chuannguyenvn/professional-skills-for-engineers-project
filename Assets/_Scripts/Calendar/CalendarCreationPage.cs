using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using Match = System.Text.RegularExpressions.Match;


namespace _Scripts.Calendar
{
    public class CalendarCreationPage : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI comment;
        [SerializeField] private TMP_Dropdown semesterDropdown;

        private MatchCollection _semesterMatchCollection;
    
        void Start()
        {
            //Adds a listener to the main input field and invokes a method when the value changes.
            //inputField.onValueChanged.AddListener(delegate
            //{
            //InputCheck();
            //    Test();
            //});
       
        }

        public void OnFastPaste()
        {
            string clipBoard = GUIUtility.systemCopyBuffer;
            inputField.text = clipBoard;
            OnInputCheck();
        }

        public void OnClearAll()
        {
            inputField.text = String.Empty;
            OnInputCheck();
        }
    
        public void OnInputCheck()
        {
            string text = inputField.text;
            string pattern =  @"Học kỳ (?<semester>\d) Năm học (?<yearFrom>\d+) - (?<yearTo>\d+)(\n|\r|\r|\n)[^(\n|\r|\r|\n)]*(\n|\r|\r|\n)[^(\n|\r|\r|\n)]*(\n|\r|\r|\n)(?<entries>(?:[^.](?!Tổng số tín chỉ đăng ký))*)";
        
            // Instantiate the regular expression object.
            Regex r = new Regex(pattern);
        
            // Match the regular expression pattern against a text string.
            _semesterMatchCollection = r.Matches(text);

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
            TimeTable timeTable = new TimeTable(Int32.Parse(groups["semester"].Value), Int32.Parse(groups["semester"].Value), groups["entries"].Value );
            TimeTableManager.Instance.AddTimeTable(timeTable);
        }

        private void SetDropdownInfo()
        {
            List<TMP_Dropdown.OptionData> semesterDropdownOptions = new List<TMP_Dropdown.OptionData>();
        
            foreach (Match match in _semesterMatchCollection)
            {
                Debug.Log("Each Match is semester"+ match.Value);
            
                TMP_Dropdown.OptionData semesterData = new TMP_Dropdown.OptionData();
                string yearFrom = match.Groups["yearFrom"].Value.Substring( match.Groups["yearFrom"].Value.Length-2);
                string semester = match.Groups["semester"].Value;
                semesterData.text = "Học Kỳ " + yearFrom + semester;
                semesterDropdownOptions.Add(semesterData);

                semesterDropdown.options = semesterDropdownOptions;
            }
        }

        private void SetCommentWarning()
        {
            comment.gameObject.SetActive(true);
        }

        private void SetActiveCommentFalse()
        {
            comment.gameObject.SetActive(false);
        }
    
    
    
    
    }
}

