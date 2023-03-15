using System;
using _Scripts.Manager;
using Map;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private Transform mapParent;
    private void Start()
    {
        foreach (var (buildingName, buildingCoordinates) in MapData.Buildings)
        {
            var building = Instantiate(ResourceManager.Instance.Building, mapParent);
            building.Init(buildingName, buildingCoordinates);
        }
    }
}