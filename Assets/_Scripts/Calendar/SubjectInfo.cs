using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class SubjectInfo
{
    public string code;
    public string name;
    public int credits;
    public string classGroup;
    [Range(2,8)] public int date;
    [Range(1,20)] public int lessonStart, lessonEnd;
    public string room;
    public List<int> weeks;
}
