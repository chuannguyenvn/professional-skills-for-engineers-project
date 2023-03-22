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
    private Dictionary<RoadIntersectionNode, DijkstraAlgorithm.Vertex> _vertices = new();

    [SerializeField] private GameObject roadNode; 
    private void Start()
    {
        InitGetBuildings();
        CreateVertexForRoadNodes();
        Test();
    }

    private void InitGetBuildings()
    {
        foreach (var building in buildingGameObjects)
        {
            _buildings.Add( building.buildingSo.name.ToLower(), building);
            //foreach (var roadIntersectionNode in building.entrances) 
            //    roadIntersectionNodes.Add(roadIntersectionNode);
        }
    }

    private void CreateVertexForRoadNodes()
    {
        //Create Vertices for each node
        foreach (var roadIntersectionNode in roadIntersectionNodes)
        {
            if (_vertices.ContainsKey(roadIntersectionNode)) continue;
            var vertex = new DijkstraAlgorithm.Vertex(roadIntersectionNode.GetInstanceID());
            _vertices.Add(roadIntersectionNode, vertex);
        }
        
        //make directed graph
        foreach (var roadIntersectionNode in roadIntersectionNodes)
        {
            Vector3 currentNodePosition = roadIntersectionNode.transform.position;
            DijkstraAlgorithm.Vertex currentNodeVertex = _vertices[roadIntersectionNode];
            foreach (var adjacentRoadNode in roadIntersectionNode.adjacentRoadNodes)
            {
                Vector3 adjacentNodePosition = adjacentRoadNode.transform.position;
                DijkstraAlgorithm.Vertex adjacentNodeVertex = _vertices[adjacentRoadNode];
                float weight = Vector3.Distance(currentNodePosition, adjacentNodePosition);
                _graphVertexList.AddEdgeDirected(currentNodeVertex,adjacentNodeVertex, weight);
            }
        }

    }

    public void Test()
    {
        Debug.Log("Test "+ roadIntersectionNodes[0].name +" is Vertex "+ _vertices[roadIntersectionNodes[0]]);
        DijkstraAlgorithm.PrintShortestPaths(_graphVertexList, _vertices[roadIntersectionNodes[0]]);
        
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