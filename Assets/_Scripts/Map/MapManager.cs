﻿using System;
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
    

    private void Start()
    {
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
        }

        /*foreach (var buildingSo in buildingScriptableObjects)
        {
            var building = Instantiate(ResourceManager.Instance.Building, mapParent);
            building.Init(buildingSo);
            buildings.Add(buildingSo.name, building);
        
        }
        */
    }
}