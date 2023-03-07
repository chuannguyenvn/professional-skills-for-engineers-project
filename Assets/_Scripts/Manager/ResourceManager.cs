using UnityEngine;

namespace _Scripts.Manager
{
    public class ResourceManager : PersistentSingleton<ResourceManager>
    {
        [Header("TimeBlock Prefabs")]
        public GameObject timeBlockSubjectGo;
        public GameObject timeBlockDayGapGo;
    }
}
