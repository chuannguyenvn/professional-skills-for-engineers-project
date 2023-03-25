using System.Collections.Generic;
using Map;
using UnityEngine;

namespace _Scripts.Map
{
    public class MapManager : Singleton<MapManager>
    {
        [SerializeField] private List<Building> buildingGameObjects;
        [SerializeField] private List<RoadIntersectionNode> roadIntersectionNodes;
        [SerializeField] private PlayerNavigation playerNavigation;

        private Dictionary<string, Building> _buildings = new();
        private PathfindingAlgorithm.GraphVertexList _graphVertexList = new();
        private Dictionary<RoadIntersectionNode, PathfindingAlgorithm.Vertex> _roadToVertices = new();
        private Dictionary<PathfindingAlgorithm.Vertex, RoadIntersectionNode> _verticesToRoad = new();
    
    
        private void Start()
        {
            InitBuildings();
            InitPlayer();
            InitVertexForRoadNodes();
        }

        #region Init
        private void InitBuildings()
        {
            foreach (var building in buildingGameObjects)
            {
                _buildings.Add( building.buildingSo.name.ToLower(), building);
                foreach (var roadIntersectionNode in building.entrances) 
                    roadIntersectionNodes.Add(roadIntersectionNode);
            }
        }
    
        private void InitPlayer()
        {
            roadIntersectionNodes.Add(playerNavigation.playerRoadNode);    
        }
    
        private void InitVertexForRoadNodes()
        {
            //Create Vertices for each node
            foreach (var roadIntersectionNode in roadIntersectionNodes)
            {
                //if (_roadToVertices.ContainsKey(roadIntersectionNode)) continue;
                var vertex = new PathfindingAlgorithm.Vertex(roadIntersectionNode.GetInstanceID());
                _roadToVertices.Add(roadIntersectionNode, vertex);
                _verticesToRoad.Add(vertex, roadIntersectionNode);
                //Debug.Log("Init "+ roadIntersectionNode.name +" is Vertex "+ _roadToVertices[roadIntersectionNode].Key+ " - "+ _roadToVertices[roadIntersectionNode] + " "+ _verticesToRoad[vertex]);
            }
        
            //make directed graph
            foreach (var roadIntersectionNode in roadIntersectionNodes)
            {
                Vector3 currentNodePosition = roadIntersectionNode.transform.position;
                PathfindingAlgorithm.Vertex currentNodeVertex = _roadToVertices[roadIntersectionNode];
                foreach (var adjacentRoadNode in roadIntersectionNode.adjacentRoadNodes)
                {
                    //Debug.Log("Adjacency "+ roadIntersectionNode.name + " and "+ adjacentRoadNode.name);
                    Vector3 adjacentNodePosition = adjacentRoadNode.transform.position;
                    PathfindingAlgorithm.Vertex adjacentNodeVertex = _roadToVertices[adjacentRoadNode];
                    AddAdjacentRoad(roadIntersectionNode, adjacentRoadNode);
                    //float weight = Vector3.Distance(currentNodePosition, adjacentNodePosition);
                    //_graphVertexList.AddEdgeDirected(currentNodeVertex,adjacentNodeVertex, weight);
                }
            }

        }

        #endregion
        public void Test()
        {
            Debug.Log("Test " + roadIntersectionNodes[0].name + " is Vertex " +
                      _roadToVertices[roadIntersectionNodes[0]].Key);
            var shortestPathsWeight =
                DijkstraAlgorithm.DijkstraShortestPathBetter(_graphVertexList, _roadToVertices[roadIntersectionNodes[0]]);

            //DijkstraAlgorithm.PrintShortestPaths(_graphVertexList, _vertices[roadIntersectionNodes[0]]);
            /*foreach (var (vertex, weight) in shortestPathsWeight)
        {
            Debug.Log("The " + _verticesToRoad[vertex].name + " Is "+ weight + "Far away");
        }
        */


            /*
        if (vertex == null || !parents.ContainsKey(vertex))
        {
            return;
        }

        PrintPath(parents[vertex], parents, path);
        Debug.Log($" {vertex.Key} ({path[vertex]})");
    */
        }

        public List<RoadIntersectionNode> ShortestPathToDestinations(RoadIntersectionNode source, RoadIntersectionNode destination)
        {
            var shortestPathsWeight =
                DijkstraAlgorithm.DijkstraShortestPathBetter(_graphVertexList, _roadToVertices[source]);
            var backTrackingVertices = PathfindingAlgorithm.backTrackingVertices;

            List<RoadIntersectionNode> roadJourney = new();
            for (PathfindingAlgorithm.Vertex traverseVertex = _roadToVertices[destination];  
                 traverseVertex != null && backTrackingVertices.ContainsKey(traverseVertex); 
                 traverseVertex = backTrackingVertices[traverseVertex])
            {
                roadJourney.Add(_verticesToRoad[traverseVertex]);
                //Debug.Log(_verticesToRoad[traverseVertex] + " " + shortestPathsWeight[traverseVertex]);
            }

            return roadJourney;
        }

        public void AddAdjacentRoad(RoadIntersectionNode source, RoadIntersectionNode destination)
        {
            float weight = Vector3.Distance(source.transform.position, destination.transform.position);
            _graphVertexList.AddEdgeDirected(_roadToVertices[source],_roadToVertices[destination], weight);
        }

        public void RemoveAdjacentRoad(RoadIntersectionNode source, RoadIntersectionNode destination)
        {
            _graphVertexList.RemoveEdge(_roadToVertices[source],_roadToVertices[destination]);
        }
    
        public Building GetBuilding(string searching)
        {
            searching = searching.ToLower();
            Debug.Log(searching +" Building");
            if (_buildings.ContainsKey(searching)) return _buildings[searching];
        
            /*
        foreach (var buildingName in _buildings.Keys)
        {
            if (buildingName.Contains(searching, StringComparison.CurrentCultureIgnoreCase ))
            {
                return _buildings[ buildingName ];
            }
        }
        */

            return null;
        }
    
        public bool Navigate(string buildingName)
        {
            Building building = GetBuilding(buildingName);
            if (building != null)
            {
                Debug.Log("Building " + building.name + " Num of Entrances " + building.entrances.Count);
                var roadJourney = ShortestPathToDestinations(playerNavigation.playerRoadNode, building.entrances[0]);
                playerNavigation.EnableNavigation(roadJourney);
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