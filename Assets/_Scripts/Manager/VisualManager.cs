using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VisualManager : Singleton<VisualManager>
{
    [Header("Map Color")] 
    [SerializeField] private Color water;
    [SerializeField] private Color road;
    [SerializeField] private Color grass;
    [SerializeField] private Color aBuilding;
    [SerializeField] private Color bBuilding;
    [SerializeField] private Color cBuilding;
    [SerializeField] private Color foodAndDrinkBuilding;
    [SerializeField] private Color garageBuilding;
    [SerializeField] private Color stadiumBuilding;
    [SerializeField] private Color facilityBuilding;
    [SerializeField] private Color otherBuilding;

    [Header("Timeblock Background")] 
    [SerializeField] private List<Sprite> timeblockBackground;

    public enum MapColor
    {
        Water,
        Road,
        Grass,
        ABuilding,
        BBuilding,
        CBuilding,
        FoodAndDrinkBuilding,
        GarageBuilding,
        StadiumBuilding, 
        FacilityBuilding,
        OtherBuilding
    }

    public Color GetMapColor(MapColor mapColor)
    {
        return mapColor switch
        {
            MapColor.Water => water,
            MapColor.Road => road,
            MapColor.Grass => grass,
            MapColor.ABuilding => aBuilding,
            MapColor.BBuilding => bBuilding,
            MapColor.CBuilding => cBuilding,
            MapColor.FoodAndDrinkBuilding => foodAndDrinkBuilding,
            MapColor.GarageBuilding => garageBuilding,
            MapColor.OtherBuilding => otherBuilding,
            MapColor.StadiumBuilding => stadiumBuilding,
            MapColor.FacilityBuilding => facilityBuilding,
            _ => Color.black
        };
    }

    public Sprite GetRandomTimeBlockBackGround()
    {
        int index = Random.Range(0, timeblockBackground.Count);
        return timeblockBackground[index];
    }
}
