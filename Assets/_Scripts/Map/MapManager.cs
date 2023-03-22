using System;
using System.Collections.Generic;
using Map;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private Transform mapParent;
    [SerializeField] private List<Building> buildingGameObjects;
    [SerializeField] private List<RoadIntersectionNode> roadIntersectionNodes;

    private Dictionary<string, Building> _buildings = new();
    private DijkstraAlgorithm.GraphVertexList _graphVertexList = new();

    [SerializeField] private GameObject roadNode; 
    private void Start()
    {
        InitGetBuildings();
    }

    private void InitGetBuildings()
    {
        foreach (var building in buildingGameObjects)
        {
            _buildings.Add( building.buildingSo.name.ToLower(), building);
            foreach (var roadIntersectionNode in building.entrances) 
                roadIntersectionNodes.Add(roadIntersectionNode);
        }
    }

    private void CreateVertexForRoadNodes()
    {
        Dictionary<RoadIntersectionNode, DijkstraAlgorithm.Vertex> vertices = new();
        foreach (var roadIntersectionNode in roadIntersectionNodes)
        {
            if (vertices.ContainsKey(roadIntersectionNode)) continue;
            var vertex = new DijkstraAlgorithm.Vertex(roadIntersectionNode.GetInstanceID());
            vertices.Add(roadIntersectionNode, vertex);
        }
        
        foreach (var roadIntersectionNode in roadIntersectionNodes)
        {
            Vector3 currentNodePosition = roadIntersectionNode.transform.position;
            DijkstraAlgorithm.Vertex currentNodeVertex = vertices[roadIntersectionNode];
            foreach (var adjacentRoadNode in roadIntersectionNode.adjacentRoadNodes)
            {
                Vector3 adjacentNodePosition = adjacentRoadNode.transform.position;
                DijkstraAlgorithm.Vertex adjacentNodeVertex = vertices[adjacentRoadNode];
                float weight = Vector3.Distance(currentNodePosition, adjacentNodePosition);
                _graphVertexList.AddEdgeDirected(currentNodeVertex,adjacentNodeVertex, weight );
            }
        }
    }

    public Building GetBuilding(string searching)
    {
        if (_buildings.ContainsKey(searching)) return _buildings[searching];
        
        foreach (var buildingName in _buildings.Keys)
        {
            if (buildingName.Contains(searching, StringComparison.CurrentCultureIgnoreCase ))
            {
                return _buildings[ buildingName ];
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

    /*
    public class ShortestPathAlgorithm
    {
        
        //private SortedSet<RoadIntersectionNode> _pQueueRoadNodes = new SortedSet<RoadIntersectionNode>(new DistanceComparer());

        public void Dijkstra(RoadIntersectionNode entranceNode)
        {
            List<(int,RoadIntersectionNode)> 
            foreach (var VARIABLE in nodes)
            {

                while (nodes.Count > 0)
                {
                    //Find smallest distance between each node
                    RoadIntersectionNode smallestDistance = nodes[0];
                    foreach (var node in nodes[0].adjacentRoadNodes)
                    {
                    
                    }
                }
            }
        }

        public List<RoadIntersectionNode> ShortestPath(List<RoadIntersectionNode> entrances, List<RoadIntersectionNode> exits)
        {
            foreach (var entrance in entrances)
            {
                Dijkstra(entrance);
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
    */
    
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