using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using _Scripts.Manager;
using Map;
using UnityEditor;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.Events;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private Transform mapParent, newMap, oldNodes, freshNodes;
    [SerializeField] private List <BuildingSO> buildingScriptableObjects;

    public Dictionary<string, Building> buildings = new();
    public List<RoadIntersectionNode> roadNodes;

    [SerializeField] private GameObject roadNode; 
    private void Start()
    {
        foreach (var buildingSo in buildingScriptableObjects)
        {
            var building = Instantiate(ResourceManager.Instance.building, mapParent);
            building.Init(buildingSo);
            buildings.Add(buildingSo.name.ToLower(), building);
        }
    }

    public void ChangeInEditor()
    {
        for (int i = 0; i < 20; i++)
        {
            var oldObj = mapParent.GetChild(i);
            var newObj = newMap.GetChild(i);

            newObj.transform.position = oldObj.transform.position;
            newObj.GetComponent<Building>()._buildingSo = oldObj.GetComponent<Building>()._buildingSo;
            
        }
        
    }

    public void ConnectRoad()
    {
        for (int i = 0; i < 53; i++)
        {
            RoadIntersectionNode oldIntersectionNode = oldNodes.GetChild(i).GetComponent<RoadIntersectionNode>();
            RoadIntersectionNode freshIntersectionNode = freshNodes.GetChild(i).GetComponent<RoadIntersectionNode>();

            //oldIntersectionNode.roadIntersectionNodes = new List<RoadIntersectionNode>();
            foreach (var oldConnectionNode in oldIntersectionNode.roadIntersectionNodes)
            {
                int oldIndex = int.Parse(oldConnectionNode.name.Substring(5));
                oldIntersectionNode.roadIntersectionNodes.Add(freshNodes.GetChild(oldIndex).GetComponent<RoadIntersectionNode>());
            }
        }
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