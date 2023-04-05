using _Scripts.Map;
using TMPro;
using UnityEngine;

namespace _Scripts.Search_Bar
{
    public class FoundItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI type;
        [SerializeField] private TextMeshProUGUI address;
        private Building _foundObject;
    
        public void Init(Building item,string typeText, string addressText)
        {
            _foundObject = item;
            type.text = typeText;
            address.text = addressText;
        }

        public Building GetObjectVariable()
        {
            return _foundObject;
        }

        public void ShowBuildingInfo()
        {
            SearchManager.Instance.OnDeselect();
            BuildingInfoManager.Instance.OnShow(_foundObject);
        }

        public void Navigate()
        {
            SearchManager.Instance.OnDeselect();
            MapManager.Instance.Navigate(_foundObject);
        }
    }
}
