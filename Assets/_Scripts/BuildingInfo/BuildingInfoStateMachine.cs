using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Manager;
using _Scripts.Map;
using _Scripts.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoStateMachine : StateMachine<BuildingInfoStateMachine, AppState>
{
    [SerializeField] private Canvas buildingInfoCanvas;
    [SerializeField] private TextMeshProUGUI buildingName, description;
    [SerializeField] private HorizontalLayoutGroup imageContent;
    private Building currentBuilding;
    private BuildingSO currentBuildingSo;
    private List<Image> _descriptiveImages = new List<Image>();

    private void Awake()
    {
        AddToFunctionQueue(()=> OnShow(currentBuilding), StateEvent.OnEnter);
    }

    public void OnShow(Building building)
    {
        Debug.Log("Show Building Info "+ building.name);
        
        buildingInfoCanvas.gameObject.SetActive(true);
        currentBuilding = building;
        currentBuildingSo = building.buildingSo;
        buildingName.text = currentBuildingSo.buildingName;
        description.text = currentBuildingSo.description;
        foreach (var descriptiveImage in _descriptiveImages)
        {
            Destroy(descriptiveImage.gameObject);
        }

        _descriptiveImages = new List<Image>();
        foreach (var sprite in currentBuildingSo.descriptiveSprites)
        {
            var descriptiveImage = Instantiate(ResourceManager.Instance.descriptiveImage, imageContent.transform);
            descriptiveImage.sprite = sprite;
            _descriptiveImages.Add(descriptiveImage);
        }
    }

    public void Navigate()
    {
        MapManager.Instance.Navigate(currentBuilding);
    }
}
