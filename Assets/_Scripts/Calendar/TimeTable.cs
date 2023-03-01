using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace _Scripts.Calendar
{
    public class TimeTable
    {
        private int _semester;
        private int _year;
        private string _info;
        public List<SubjectInfo> subjectInfos = new();

        public TimeTable(int semester, int year, string info)
        {
            _semester = semester;
            _year = year;
            _info = info;
            DecodeInfo();
        }

        private void DecodeInfo()
        {
            
            string[] subjectLineStrings = _info.Split("\n");

            int startYear = _semester == 1 ? _year : _year + 1;
                
            // There is 2 line "\n" and "MÃ MH	TÊN MÔN HỌC	TÍN CHỈ	TC HỌC PHÍ	NHÓM-TỔ	THỨ	TIẾT	GIỜ HỌC	PHÒNG	CƠ SỞ	TUẦN HỌC" that don't need for subject
            for (int i = 2; i < subjectLineStrings.Length; i++)
            {
                
                SubjectInfo subjectInfo = new(startYear,subjectLineStrings[i]);
                subjectInfos.Add(subjectInfo);
            }
        }
    }
}