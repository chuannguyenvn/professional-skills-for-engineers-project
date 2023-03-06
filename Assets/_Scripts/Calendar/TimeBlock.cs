using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeBlock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI firstHeader;
    [SerializeField] private TextMeshProUGUI secondHeader;
    
    public void Init(SubjectInfo subjectInfo)
    {
        firstHeader.text = subjectInfo.name;
        secondHeader.text = subjectInfo.lessonStartHour.ToString() + " - " + subjectInfo.lessonEndHour.ToString() +
                            " at " + subjectInfo.room;
    }
}
