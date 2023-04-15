using _Scripts.Map;
using _Scripts.Search_Bar;
using Shapes;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Manager
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        [Header("TimeBlock Prefabs")]
        public TimeBlockSubject timeBlockSubject;
        public TimeBlockSubject timeBlockOldSubject;
        public TimeBlockWeekGap timeBlockWeekGap;

        [Header("Map")]
        public Map.Building building;
        public RoadIntersectionNode roadIntersectionNode;

        [Header("Search Prefabs")] public FoundItem foundItem;

        [Header("Building Info")] public Image descriptiveImage;

        public Polyline Polyline;
    }
}
