using System.Collections.Generic;
using System.Linq;
using _Scripts.Manager;
using _Scripts.Map;
using _Scripts.Search_Bar;
using _Scripts.StateMachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.State
{
    public class SearchStateMachine : StateMachine<SearchStateMachine, AppState>
    {
        [Header("Search UI")] [SerializeField] private CanvasGroup _homeMenuCanvas;
        [SerializeField] private CanvasGroup _searchMenuCanvas;
        [SerializeField] private TMP_InputField _trueSearchBar;
        [SerializeField] private TMP_InputField _homeSearchBar;
        [SerializeField] private Button _backButton;
        [SerializeField] private float _transitionDuration;

        [Header("Search Contain")] [SerializeField]
        private VerticalLayoutGroup _foundContents;

        [SerializeField, Range(0, 10)] private int _maxFoundElement;
        private List<FoundItem> _foundSearchItems = new();

        private void Awake()
        {
            InitEvent();
            AddToFunctionQueue(OnSelect, StateEvent.OnEnter);
            AddToFunctionQueue(OnDeselect, StateEvent.OnExit);
        }

        private void InitEvent()
        {
            _trueSearchBar.onValueChanged.AddListener(OnSearchValueChanged);
            _backButton.onClick.AddListener(() => ApplicationManager.Instance.SetState(AppState.Home));
        }

        public void OnSearchValueChanged(string text)
        {
            text = Utility.RemoveSpecialVietnameseSigns(text);

            _homeSearchBar.text = text;

            foreach (var foundSearchItem in _foundSearchItems)
            {
                Destroy(foundSearchItem.gameObject);
            }

            _foundSearchItems.Clear();

            List<Building> freshFoundBuildings = MapManager.Instance.FindBuildings(text);

            foreach (var building in freshFoundBuildings)
            {
                var newItem =
                    Instantiate(ResourceManager.Instance.foundItem,
                        _foundContents.transform); // Create a new FoundItem object using Instantiate
                newItem.Init(building, "Building", building.buildingSo.buildingName);
                _foundSearchItems.Add(newItem); // Add the new object to the list
            }
        }


        public void OnSelect(AppState exitState)
        {
            Debug.Log("Active state search");
            
            if (exitState == AppState.Search) return;
            _searchMenuCanvas.gameObject.SetActive(true);

            _trueSearchBar.ActivateInputField();
            
            _searchMenuCanvas.alpha = 0;
            DOTween.To(() => _searchMenuCanvas.alpha, x => _searchMenuCanvas.alpha = x, 1, _transitionDuration);
        }

        public void OnDeselect(AppState enterState)
        {
            //_trueSearchBar.DeactivateInputField();
            if (enterState == AppState.Search) return;
            
            _searchMenuCanvas.alpha = 1;
            DOTween.To(() => _searchMenuCanvas.alpha, x => _searchMenuCanvas.alpha = x, 0, _transitionDuration)
                .OnComplete(() => _searchMenuCanvas.gameObject.SetActive(false));
            
        }
    }
}