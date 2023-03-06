using System.Collections.Generic;

namespace _Scripts.Calendar
{
    public class DataManager : PersistentSingleton<DataManager>
    {
        private List<TimeTable> _timeTables = new();
        
        public void AddTimeTable(TimeTable addingTimeTable)
        {
            _timeTables.Add(addingTimeTable);
        }

        public TimeTable GetTimeTable(int index = 0)
        {
            if (index < 0 || index >= _timeTables.Count) return null;
            return _timeTables[index] ;
        }
        
    }
}