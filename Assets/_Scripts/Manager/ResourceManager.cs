using UnityEngine;

namespace _Scripts.Manager
{
    public class ResourceManager : PersistentSingleton<ResourceManager>
    {
        [Header("Prefabs")]
        public GameObject timeBlockSubjectGo;
    }
}
