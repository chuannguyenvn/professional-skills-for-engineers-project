using System;
using System.Collections.Generic;
using _Scripts.Manager;
using Map;
using UnityEditor;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private Transform mapParent;
    [SerializeField] private List <BuildingSO> buildingScriptableObjects;

    public Dictionary<string, Building> buildings = new();
    public List<RoadIntersectionNode> roadNodes; 

    private void Start()
    {
        foreach (var buildingSo in buildingScriptableObjects)
        {
            var building = Instantiate(ResourceManager.Instance.building, mapParent);
            building.Init(buildingSo);
            buildings.Add(buildingSo.name, building);
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