using _Scripts.Manager;
using _Scripts.Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Search_Bar
{
    public class FoundItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI type;
        [SerializeField] private TextMeshProUGUI address;
        [SerializeField] private Button panelButton;
        [SerializeField] private Button navigationButton;
        private Building _foundObject;
    
        public void Init(Building item,string typeText, string addressText)
        {
            _foundObject = item;
            type.text = typeText;
            address.text = addressText;
            
            panelButton.onClick.AddListener(() => ApplicationManager.Instance.SetState(AppState.Info, null, new object[]{_foundObject}));
            navigationButton.onClick.AddListener(() => ApplicationManager.Instance.SetState(AppState.Navigate, null, new object[]{_foundObject}));
        }

        public Building GetObjectVariable()
        {
            return _foundObject;
        }

        public void ShowBuildingInfo()
        {
            //SearchManager.Instance.OnDeselect();
            //BuildingInfoStateMachine.Instance.OnShow(_foundObject);
        }

        public void Navigate()
        {
            //SearchManager.Instance.OnDeselect();
            //MapManager.Instance.Navigate(_foundObject);
        }
    }
}
