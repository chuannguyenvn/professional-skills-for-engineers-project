using _Scripts.Map;
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
        public RoadIntersectionNode roadIntersectionNode;

        [Header("Search Prefabs")] public FoundItem foundItem;
    }
}
