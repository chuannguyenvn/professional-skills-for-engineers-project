using System;
using System.Collections.Generic;
using _Scripts.Manager;
using Map;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private Transform mapParent;
    [SerializeField] private List <BuildingSO> buildingScriptableObjects;

    public Dictionary<string, Building> buildings = new();

    private void Start()
    {
        //foreach (var (buildingName, buildingCoordinates) in MapData.Buildings)
        //{
        //    var building = Instantiate(ResourceManager.Instance.Building, mapParent);
        //    building.Init(buildingName, buildingCoordinates);
        //}

        foreach (var buildingSo in buildingScriptableObjects)
        {
            var building = Instantiate(ResourceManager.Instance.Building, mapParent);
            building.Init(buildingSo);
            buildings.Add(buildingSo.name, building);
        }
    }
}