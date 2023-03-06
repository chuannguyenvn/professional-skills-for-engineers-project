using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeBlockSubject : TimeBlock<SubjectInfo>
{
    [SerializeField] private TextMeshProUGUI firstHeader;
    [SerializeField] private TextMeshProUGUI secondHeader;

    public override void Init(SubjectInfo subjectInfo)
    {
        firstHeader.text = subjectInfo.name;
        secondHeader.text = subjectInfo.lessonStartHour.Hours + ":" + subjectInfo.lessonStartHour.Minutes.ToString("D2") + " - " +
                            subjectInfo.lessonEndHour.Hours + ":" + subjectInfo.lessonEndHour.Minutes.ToString("D2") +
                            " tại " + subjectInfo.room;
    }
    
}