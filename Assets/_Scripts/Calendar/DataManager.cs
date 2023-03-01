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
        
        
    }
}