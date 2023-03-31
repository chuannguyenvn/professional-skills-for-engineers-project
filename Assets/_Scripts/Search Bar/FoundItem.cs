using System.Collections;
using System.Collections.Generic;
using _Scripts.Map;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FoundItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI type;
    [SerializeField] private TextMeshProUGUI address;
    private Building _foundObject;
    
    public void Init(Building item,string typeText, string addressText)
    {
        _foundObject = item;
        this.type.text = typeText;
        this.address.text = addressText;
    }

    public Building GetObjectVariable()
    {
        return _foundObject;
    }

    public void ShowDescriptiveBuilding()
    {
        BuildingInfoManager.Instance.OnShow(_foundObject);
    }
}
