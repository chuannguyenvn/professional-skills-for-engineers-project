using System;
using _Scripts.Manager;
using Map;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    private void Start()
    {
        foreach (var (buildingName, buildingCoordinates) in MapData.Buildings)
        {
            var building = Instantiate(ResourceManager.Instance.Building);
            building.Init(buildingName, buildingCoordinates);
        }
    }
}