using Shapes;
using UnityEngine;

namespace _Scripts.Manager
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        [Header("TimeBlock Prefabs")]
        public TimeBlockSubject timeBlockSubject;
        public TimeBlockDayGap timeBlockDayGap;

        [Header("Map")]
        public Map.Building building;
        public RoadNode roadNode;
    }
}
