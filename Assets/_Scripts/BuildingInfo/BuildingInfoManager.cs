using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoManager : Singleton<BuildingInfoManager>
{
    [SerializeField] private Canvas buildingInfoCanvas;
    [SerializeField] private TextMeshProUGUI buildingName, description;
    [SerializeField] private HorizontalLayoutGroup imageContent;
    public BuildingSO currentBuilding;
    private List<Image> _descriptiveImages = new List<Image>();

    public void OnShow(BuildingSO buildingSo)
    {
        Debug.Log("Show Building Info "+ buildingSo.name);
        buildingInfoCanvas.gameObject.SetActive(true);
        currentBuilding = buildingSo;
        buildingName.text = buildingSo.name;
        description.text = buildingSo.description;
        foreach (var descriptiveImage in _descriptiveImages)
        {
            Destroy(descriptiveImage.gameObject);
        }

        _descriptiveImages = new List<Image>();
        foreach (var sprite in buildingSo.descriptiveSprites)
        {
            var descriptiveImage = Instantiate(ResourceManager.Instance.descriptiveImage, imageContent.transform);
            descriptiveImage.sprite = sprite;
            _descriptiveImages.Add(descriptiveImage);
        }
    }
}
