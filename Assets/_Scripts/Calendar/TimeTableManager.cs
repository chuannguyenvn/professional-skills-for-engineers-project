using System.Collections.Generic;

namespace _Scripts.Calendar
{
    public class TimeTableManager : PersistentSingleton<TimeTableManager>
    {
        private List<TimeTable> _timeTables;

        public void AddTimeTable(TimeTable addingTimeTable)
        {
            _timeTables.Add(addingTimeTable);
        }
    }
}