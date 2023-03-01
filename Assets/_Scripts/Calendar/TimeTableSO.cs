using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeTable", menuName = "ScriptableObjects/TimeTable", order = 1)]
public class TimeTableSO : ScriptableObject
{
    public int semester;
    public int year;
    public List<SubjectInfo> subjectInfos;

    
    public void Init(int _semester, int _year, List< SubjectInfo > _subjectInfos)
    {
        semester = _semester;
        year = _year;
        subjectInfos = _subjectInfos;
    }
    
    
    
}
