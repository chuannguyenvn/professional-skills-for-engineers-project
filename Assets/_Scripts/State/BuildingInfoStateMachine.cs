using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Manager;
using _Scripts.Map;
using _Scripts.StateMachine;
using _Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoStateMachine : StateMachine<BuildingInfoStateMachine, AppState>
{
    [SerializeField] private Canvas buildingInfoCanvas;
    [SerializeField] private TextMeshProUGUI buildingName, description;
    [SerializeField] private HorizontalLayoutGroup imageContent;
    [SerializeField] private SwipeVerticalMenu buildingInfoSwipe;
    
    private Building currentBuilding;
    private BuildingSO currentBuildingSo;
    private List<Image> _descriptiveImages = new List<Image>();

    private void Awake()
    {
        AddToFunctionQueue(OnSelect, StateEvent.OnEnter);
        AddToFunctionQueue(OnDeselect, StateEvent.OnExit);
        buildingInfoSwipe?.verticalPages[0]?.onSelectEvent.AddListener( () => ApplicationManager.Instance.SetState(AppState.Home));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter">
    /// [0] : Building
    /// </param>
    public IEnumerator OnSelect(AppState exitState, object[] parameter)
    {
        //Debug.Log("Info enter");
        Building building = parameter[0] as Building;
        
        if(exitState != AppState.Info) buildingInfoCanvas.gameObject.SetActive(true);
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
        
        yield return null;
    }

    public void OnDeselect(AppState enterState)
    {
        //Debug.Log("Info exit");
        if(enterState != AppState.Info) buildingInfoCanvas.gameObject.SetActive(false);
    }

    public void Navigate()
    {
        MapManager.Instance.Navigate(currentBuilding);
    }
}
