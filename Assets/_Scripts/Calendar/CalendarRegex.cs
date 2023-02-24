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
        //inputField.onValueChanged.AddListener(delegate
        //{
            //InputCheck();
        //    Test();
        //});
       
    }

    public void InputCheck()
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
    }

    public void Test()
    {

        string text = inputField.text;
	   
        // Orgininal but bug ?
	  //string pattern = @"Học kỳ (?<semester>\d) Năm học (?<yearFrom>\d+) - (?<yearTo>\d+)\n[^\n]*\n[^\n]*\n(?<entries>(?:[^]*(?!Tổng số tín chỉ đăng ký))*)";
        
        string pattern = @"Học kỳ (?<semester>\d) Năm học (?<yearFrom>\d+) - (?<yearTo>\d+)\n[^\n]*\n[^\n]*\n(?<entries>(?:.*(?!Tổng số tín chỉ đăng ký))*)";
        
        // Instantiate the regular expression object.
        Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
        
        // Match the regular expression pattern against a text string.
        var matches = r.Matches(text);
        
          
        foreach (Match match in matches){
            Debug.Log("Match");
          
        }
      
        //TEST CASE 2 CAN DETECT 5 CASE  
        string pat2 = @"Học kỳ (?<semester>\d) Năm học (?<yearFrom>\d+) - (?<yearTo>\d+)";

        Regex r2 =  new Regex(pat2, RegexOptions.IgnoreCase);
        var matches2 = r2.Matches(text);
        foreach (Match match in matches2){
            Debug.Log("Match2");
          
        }

        //TEST CASE 3 CANNOT DETECT DUE TO \n AT THE END???
        string pat3 = @"Học kỳ (?<semester>\d) Năm học (?<yearFrom>\d+) - (?<yearTo>\d+)\n";

        Regex r3 =  new Regex(pat3, RegexOptions.IgnoreCase);
        var matches3 = r3.Matches(text);
        foreach (Match match in matches3){
            Debug.Log("Match3");
          
        }

        // TEST CASE TXT4 CAN DETECT THE \n
        string txt4 = @"


";
        for(int i = 0 ; i< txt4.Length; i++){
            if(txt4[i]=='\n')  Debug.Log("newline");
        }
      
        /*
        while (m.Success)
        {
           Debug.Log("Match"+ (++matchCount));
           for (int i = 1; i <= 2; i++)
           {
              Group g = m.Groups[i];
              Debug.Log("Group"+i+"='" + g + "'");
              CaptureCollection cc = g.Captures;
              for (int j = 0; j < cc.Count; j++)
              {
                 Capture c = cc[j];
                 System.Debug.Log("Capture"+j+"='" + c + "', Position="+c.Index);
              }
           }
           m = m.NextMatch();
        }
        */

    }
    
}

