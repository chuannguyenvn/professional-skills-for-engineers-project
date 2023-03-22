using System;
using System.Collections.Generic;
using Map;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private Transform mapParent, newMap, oldNodes, freshNodes;
    [SerializeField] private List <BuildingSO> buildingScriptableObjects;

    public Dictionary<string, Building> buildings = new();
    public List<RoadIntersectionNode> roadNodes;

    [SerializeField] private GameObject roadNode; 
    private void Start()
    {
        
    }


    public Building GetBuilding(string searching)
    {
        if (buildings.ContainsKey(searching)) return buildings[searching];
        
        foreach (var buildingName in buildings.Keys)
        {
            if (buildingName.Contains(searching, StringComparison.CurrentCultureIgnoreCase ))
            {
                return buildings[ buildingName ];
            }
        }

        return null;
    }

    

    public bool Navigate(string room)
    {
        Building building = GetBuilding(room);
        if (building != null)
        {
            Debug.Log(room+" "+ building.name);        
            return true;
        }
        else
        {
            return false;
        }

    }

    

    #region Dijkstra

    public class ShortestPathAlgorithm
    {
        
        //private SortedSet<RoadIntersectionNode> _pQueueRoadNodes = new SortedSet<RoadIntersectionNode>(new DistanceComparer());

        public void Dijkstra(List<RoadIntersectionNode> entranceNodes)
        {
            List<RoadIntersectionNode> nodes = new List<RoadIntersectionNode>( entranceNodes );
            
            while (nodes.Count > 0)
            {
                //Find smallest distance between each node
                RoadIntersectionNode smallestDistance = null;
                foreach (var node in nodes[0].adjacentRoadNodes)
                {

                }
            }
        }
        
        private class DistanceComparer : IComparer<RoadIntersectionNode>
        {
            public int Compare(int[] x, int[] y)
            {
                if (x[0] == y[0]) {
                    return x[1] - y[1];
                }
                return x[0] - y[0];
            }

            public int Compare(RoadIntersectionNode x, RoadIntersectionNode y)
            {
                return -1;
            }
        }
    }
    
    #endregion
    
    
    #region Unused

    private void CreateBuildingPrefab(Building building)
    {
        // Create some asset folders.
        //AssetDatabase.CreateFolder("Assets/Prefabs", "BuildingPrefabs");
            
        // The paths to the mesh/prefab assets.
        //string prefabPath = "Assets/Prefabs/BuildingPrefabs/"+ building.name +".prefab";
 
        // Delete the assets if they already exist.
        //AssetDatabase.DeleteAsset(prefabPath);
            
        // Save the transform's GameObject as a prefab asset.
        //PrefabUtility.CreatePrefab(prefabPath, building.gameObject);

    }    
    

    #endregion
}

/*  Create Scriptable Object
    
    foreach (var (buildingName, buildingCoordinates) in MapData.Buildings)
        {
            BuildingSO example = ScriptableObject.CreateInstance<BuildingSO>();
            string path = "Assets/ScriptableObject/Buildings/" + buildingName+".asset";

            example.name = buildingName;
            example.geoCoordinate = new List<Vector2>(buildingCoordinates);
            AssetDatabase.CreateAsset(example, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = example;
        }*/