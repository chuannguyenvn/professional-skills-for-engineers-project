using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class CalendarRegex : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI comment;

    class TimeTable
    {
        
    }
    void Start()
    {
        //Adds a listener to the main input field and invokes a method when the value changes.
        inputField.onValueChanged.AddListener(delegate {InputCheck(); });
       
    }

    void InputCheck()
    {
        string text = "";
        string pattern = @"Học kỳ (?<semester>\d) Năm học (?<yearFrom>\d+) - (?<yearTo>\d+)\n[^\n]*\n[^\n]*\n(?<entries>(?:[^](?!Tổng số tín chỉ đăng ký))*)";
        string[] digits = Regex.Split(text, @"\D+");

        foreach (string value in digits)
        {
            int number;
            if (int.TryParse(value, out number))
            {
                Console.WriteLine(value);
            }
        }
    }

}

