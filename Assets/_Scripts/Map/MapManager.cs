using System;
using System.Collections.Generic;
using _Scripts.Manager;
using Map;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private Transform mapParent;
    [SerializeField] private Transform intersectionNodeParent;
    [SerializeField] private List <BuildingSO> buildingScriptableObjects;

    public Dictionary<string, Building> buildings = new();
    public List<RoadIntersectionNode> roadNodes; 

    private void Start()
    {
        foreach (var buildingSo in buildingScriptableObjects)
        {
            var building = Instantiate(ResourceManager.Instance.building, mapParent);
            building.Init(buildingSo);
            buildings.Add(buildingSo.name.ToLower(), building);
        }
    }

    public void Temp()
    {
        for (int i = 0; i < intersectionNodeParent.childCount; i++)
        {
            var road = Instantiate( ResourceManager.Instance.roadIntersectionNode, intersectionNodeParent.transform);
            road.transform.position = intersectionNodeParent.GetChild(i).transform.position;
            road.GetComponent<RoadIntersectionNode>().roadIntersectionNodes = intersectionNodeParent.GetChild(i)
                .GetComponent<RoadIntersectionNode>().roadIntersectionNodes;
            road.name = "Road Node "+i.ToString();
            
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