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
        InputCheck();
    }

    public void InputCheck()
    {
        string pattern = "pdt@hcmut.edu.vn";
        
    }

    private void MakeTimetable()
    {
        string text = inputField.text;
        string pattern =  @"Học kỳ (?<semester>\d) Năm học (?<yearFrom>\d+) - (?<yearTo>\d+)(\n|\r|\r|\n)[^(\n|\r|\r|\n)]*(\n|\r|\r|\n)[^(\n|\r|\r|\n)]*(\n|\r|\r|\n)(?<entries>(?:[^.](?!Tổng số tín chỉ đăng ký))*)";
        
        // Instantiate the regular expression object.
        Regex r = new Regex(pattern);
        
        // Match the regular expression pattern against a text string.
        var matches = r.Matches(text);

        foreach (Match match in matches)
        {
            Debug.Log("Match "+ match.Value);
            foreach (Group group in match.Groups)
            {
                Debug.Log("Group "+group.Value);
                foreach (Capture capture in group.Captures)
                {
                    Debug.Log("Capture "+ capture);
                }
            }
        }
        
        //TimeTable timeTable = new TimeTable();
    } 
    
    
    
    
}

