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
        secondHeader.text = subjectInfo.lessonStartHour.Hours + ":" + subjectInfo.lessonStartHour.Minutes.ToString("D2") + " - " +
                            subjectInfo.lessonEndHour.Hours + ":" + subjectInfo.lessonEndHour.Minutes.ToString("D2") +
                            " tại " + subjectInfo.room;
    }
}