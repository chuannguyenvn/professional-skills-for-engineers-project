using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;


public class CalendarCreationPage : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI comment;
    [SerializeField] private TMP_Dropdown semesterDropdown;
    
    class TimeTable
    {
        
    }
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
        var matches = r.Matches(text);

        List<TMP_Dropdown.OptionData> semesterDropdownOptions = new List<TMP_Dropdown.OptionData>();
        
        foreach (Match match in matches)
        {
            Debug.Log("Each Match is semester"+ match.Value);
            
            TMP_Dropdown.OptionData semesterData = new TMP_Dropdown.OptionData();
            string yearFrom = match.Groups["yearFrom"].Value.Substring( match.Groups["yearFrom"].Value.Length-2);
            string semester = match.Groups["semester"].Value;
            semesterData.text = "Học Kỳ " + yearFrom + semester;
            semesterDropdownOptions.Add(semesterData);
            /*
            foreach (Group group in match.Groups)
            {
                Debug.Log("Group "+group.Value);
                foreach (Capture capture in group.Captures)
                {
                    Debug.Log("Capture "+ capture);
                }
            }
            */

            semesterDropdown.options = semesterDropdownOptions;
        }
    } 
    
    
    
    
}

