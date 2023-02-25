using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTable 
{
    private int _semester;
    private int _year;
    private string _info;
    public List<SubjectInfo> subjectInfos;

    public TimeTable(int semester, int year, string info)
    {
        _semester = semester;
        _year = year;
        _info = info;
        
        DecodeInfo();
    }

    private void DecodeInfo()
    {
        
    }



}
